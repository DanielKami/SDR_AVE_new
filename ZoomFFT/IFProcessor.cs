using System;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using SDRSharp.Common;
using SDRSharp.PanView;
using SDRSharp.Radio;
using System.IO;
using System.Text;
using System.Windows;

namespace SDRSharp.Average
{
    public unsafe class IFProcessor : IIQProcessor
    {
        //Window
        int window_Height=400, window_Width=600;
        public int Max_BufferSize = 1024 / 4;
        public float Gain = 70;
        public float Level = 100;
        private int average_max = 50000;
        public int average = 10;
        private int cumulateIndex = 0;
        public int Intermediate_average = 10;
        private int Intermediate_cumulateIndex = 0;
        public bool background_recording=false;
        public bool background_corrected = false;
        private UnsafeBuffer _iqBuffer;
        private Complex* _iqPtr;

        private UnsafeBuffer PostProcBuffer;
        private double* _PostProcPtr;
        private double[] background_buffer;

        private UnsafeBuffer ExportBuffer;
        private double* _ExportPtr;


        private UnsafeBuffer CumulateBuffer_;
        private float* CumulateBuffer;

        private UnsafeBuffer _IntermediateBuffer;
        private float* IntermediateBuffer;

        private UnsafeBuffer _FourierBufferIn;
        private Complex* FourierBufferIn;

        private UnsafeBuffer _FourierBufferOut;
        private Complex* FourierBufferOut;

        private double _sampleRate;
        private Thread _fftThread;
        private bool _ThreadRunning;
        private System.Windows.Forms.Timer _Timer;
        private IFAverageWindow _IFAverageWindow;

        public FFT mFFT;
        private float max = 0;
        //private readonly float _fftOffset = (float) Utils.GetDoubleSetting("fftOffset", -40.0);
        private readonly SharpEvent _Event = new SharpEvent(false);
        private readonly ComplexFifoStream _iqStream = new ComplexFifoStream(BlockMode.BlockingRead);
        private readonly ISharpControl _control;

        public bool AverageWindowOn = false;

        public IFProcessor(ISharpControl control)
        {

            _control = control;
            _control.PropertyChanged += NotifyPropertyChangedHandler;
            Enabled = true;
            max = 0;
           
            RestartIFWindow();

            mFFT = new FFT();

            #region FFT window Timer
            _Timer = new System.Windows.Forms.Timer();
            _Timer.Interval = 33;
            _Timer.Tick += Timer_Tick;
            #endregion

            #region Buffers
            InitBuffers();
            #endregion

            //Which streem to recive
            _control.RegisterStreamHook(this, ProcessorType.RawIQ);
        }

        public void RestartIFWindow()
        {

            if (_IFAverageWindow != null)
            {
                window_Width = _IFAverageWindow.Width;
                window_Height = _IFAverageWindow.Height;

                _IFAverageWindow.Dispose();
            }
            try
            {
                _IFAverageWindow = new IFAverageWindow();
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with opening window" + e.Source, "Important Message");
            }
            
           
            _IFAverageWindow.frequency = _control.CenterFrequency;
            _IFAverageWindow.rate = _control.RFBandwidth;
            _IFAverageWindow.BufferFrameSize = Max_BufferSize;
            _IFAverageWindow.Data = _PostProcPtr;
            _IFAverageWindow.Gain = Gain;
            _IFAverageWindow.Level = Level;

            _IFAverageWindow.Width = window_Width;
            _IFAverageWindow.Height = window_Height;

            _IFAverageWindow.PanelSizechanged();

        }


        public void ControlPassiveRadarWindow()
        {
            AverageWindowOn = true;
            _IFAverageWindow.Show();
            Start();
        }

        public void ControlPassiveRadarWindowHide()
        {
            AverageWindowOn = false;
            _IFAverageWindow.Hide();
            Stop();
        }

        public double SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (_sampleRate != value)
                {
                    _sampleRate = value;
                }
            }
        }

        public bool Enabled
        {
            get;
            set;
        }

        public void Process(Complex* buffer, int length)
        {
            if (_iqStream.Length < length * 4)
            {
                _iqStream.Write(buffer, length);
            }

        }

        private void NotifyPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "StartRadio":
                    RestartIFWindow();
                    Thread.Sleep(500);

                    if (_IFAverageWindow.Enabled & AverageWindowOn)
                        ControlPassiveRadarWindow();
                    break;

                case "StopRadio":
                    Stop();
                    break;

            }
            max = 0;
            if (_IFAverageWindow != null && _IFAverageWindow.Visible)
            {
                _IFAverageWindow.ScaleUpdate();
                _IFAverageWindow.frequency = _control.CenterFrequency;
                _IFAverageWindow.rate = _control.RFBandwidth;
                _IFAverageWindow.Invalidate();
                _IFAverageWindow.Gain = Gain;
                _IFAverageWindow.Level = Level;
                _IFAverageWindow.Render();
            }

        }

        public void Reset()
        {
            for (int i = 0; i < Max_BufferSize * average_max; i++)
            {
                CumulateBuffer[i] = 0;
            }
            cumulateIndex = 0;

            for (int i = 0; i < Max_BufferSize; i++)
                background_buffer[i] = 1;
            background_corrected = false;
        }

        public void DataClean()
        {
            for (int i = 0; i < Max_BufferSize * average_max; i++)
            {
                CumulateBuffer[i] = 0;
            }
            cumulateIndex = 0;

 
        }
        #region Thread

        private void Start()
        {
            _ThreadRunning = true;

            if (_fftThread == null)
            {
                _fftThread = new Thread(ProcessFFT);
                _fftThread.Name = "IF Average";
                _fftThread.Start();
                _iqStream.Open();
                mFFT.PrepareFFT();
            }

            // _Timer.Enabled = true;
            _Timer.Start();
        }

        public void Stop()
        {
            _Timer.Stop();
            //_Timer.Enabled = false;

            _ThreadRunning = false;

            if (_fftThread != null)
            {
                _iqStream.Close();
                _Event.Set();
                _fftThread.Join();
                _fftThread = null;
            }
        }

        #endregion

        #region Proces thread

        private void ProcessFFT(object parameter)
        {
            while (_control.IsPlaying && _ThreadRunning)
            {

                #region Read IQ data a correct way

                var total = 0;

                while (_control.IsPlaying && _ThreadRunning && total < Max_BufferSize)
                {
                    var len = Max_BufferSize - total;
                    //Read reads all available data to len. If len is larger than available data then stop and return new total of read data. After reading len is shorter because we have less data to read                   
                    total += _iqStream.Read(_iqPtr, Max_BufferSize - len, len);
                }

                #endregion

                #region FFT
                //copy buffer 
                for (int i = 0; i < Max_BufferSize; i++)
                    FourierBufferIn[i] = _iqPtr[i];


                //calculate FFT
                mFFT.CalcFFT(FourierBufferIn, FourierBufferOut, Max_BufferSize);
                #endregion
                #region Average
                //Intermediate cumulation FFT buffer
                Intermediate_cumulateIndex++;

                for (int i = 0; i < Max_BufferSize; i++)
                    IntermediateBuffer[i] += FourierBufferOut[i].Modulus();



                if (Intermediate_cumulateIndex >= Intermediate_average)
                {
                    Intermediate_cumulateIndex = 0;

                    cumulateIndex++;
                    if (cumulateIndex >= average)
                    {
                        cumulateIndex = 0;
                        background_recording = false; //if average buffer is completed stop background aquisition
                    }

                    //Order buffer data one half on left other on right                    
                    float temp;
                    int hb = Max_BufferSize / 2;
                    for (int i = 0; i < hb; i++)
                    {
                        temp = IntermediateBuffer[i];
                        IntermediateBuffer[i] = IntermediateBuffer[i + hb];
                        IntermediateBuffer[i + hb] = temp;
                    }

                    //Reverse IQ
                    if (!_control.SwapIq)
                    {
                        int pos;
                        for (int i = 0; i < hb; i++)
                        {
                            pos = Max_BufferSize - i;
                            temp = IntermediateBuffer[i];
                            IntermediateBuffer[i] = IntermediateBuffer[pos];
                            IntermediateBuffer[pos] = temp;
                        }
                    }
                    temp = 1.0f / Intermediate_average;
                    for (int i = 0; i < Max_BufferSize; i++)
                    {
                        IntermediateBuffer[i] *= temp;
                    }


                    if (_control.SquelchEnabled)
                    {
                        max = 0;
                        _IFAverageWindow.ScaleUpdate();
                    }
                    for (int i = 0; i < Max_BufferSize; i++)
                    {
                        if (IntermediateBuffer[i] > max) max = IntermediateBuffer[i];
                    }

                    //Scale to max
                    int Cm = cumulateIndex * Max_BufferSize;

                    ///Old version
                    //float scale = 1.0f / max;
                    //for (int i = 0; i < Max_BufferSize; i++)
                    //    CumulateBuffer[Cm + i] = (float)Math.Log10(IntermediateBuffer[i] * scale);

                    ////Reset intermediate buffer
                    //for (int i = 0; i < Max_BufferSize; i++)
                    //    IntermediateBuffer[i] = 0;


                    for (int i = 0; i < Max_BufferSize; i++)
                        CumulateBuffer[Cm + i] = IntermediateBuffer[i];

                    //Reset intermediate buffer
                    for (int i = 0; i < Max_BufferSize; i++)
                        IntermediateBuffer[i] = 0;


                }
                #endregion

            }

            _iqStream.Flush();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_control.IsPlaying)
            {
                float res;
                float temp = 1.0f / average;
                for (int i = 0; i < Max_BufferSize; i++)
                {
                    res = 0;
                    for (int j = 0; j < average; j++)
                        res += CumulateBuffer[j * Max_BufferSize + i];


                    if (background_recording == true)
                    {
                        background_buffer[i] = res;
                        _PostProcPtr[i] = Math.Log10(_ExportPtr[i] = res * temp) * Gain - 35 + Level;

                    }
                    else
                    {
                        _PostProcPtr[i] = Math.Log10(_ExportPtr[i] = res / background_buffer[i] * temp) * Gain - 35 + Level;
                    }
                }





                if (_IFAverageWindow != null && _IFAverageWindow.Visible)
                {
                    _IFAverageWindow.frequency = _control.CenterFrequency;
                    _IFAverageWindow.rate = _control.RFBandwidth;
                    _IFAverageWindow.cumulation = cumulateIndex * Intermediate_average;
                    _IFAverageWindow.cumulation_max = average * Intermediate_average;
                    _IFAverageWindow.Invalidate();
                    _IFAverageWindow.Gain = Gain;
                    _IFAverageWindow.Level = Level;
                    _IFAverageWindow.background_recording = background_recording;
                    if (background_recording)
                        background_corrected = true;
                    _IFAverageWindow.background_corrected = background_corrected;
                    _IFAverageWindow.Render();
                }
            }
        }

        private void InitBuffers()
        {
            if (_iqBuffer != null) _iqBuffer.Dispose();
            _iqBuffer = UnsafeBuffer.Create(Max_BufferSize, sizeof(Complex));
            _iqPtr = (Complex*)_iqBuffer;

            if (CumulateBuffer_ != null) CumulateBuffer_.Dispose();
            CumulateBuffer_ = UnsafeBuffer.Create(Max_BufferSize * (average_max + 1), sizeof(double));
            CumulateBuffer = (float*)CumulateBuffer_;


            // MessageBox.Show("Dot Net Perls is awesome.");
            if (ExportBuffer != null) ExportBuffer.Dispose();
            ExportBuffer = UnsafeBuffer.Create(Max_BufferSize, sizeof(double));
            _ExportPtr = (double*)ExportBuffer;

            if (PostProcBuffer != null) PostProcBuffer.Dispose();
            PostProcBuffer = UnsafeBuffer.Create(Max_BufferSize, sizeof(double));
            _PostProcPtr = (double*)PostProcBuffer;

            background_buffer = new double[Max_BufferSize];
            for (int i = 0; i < Max_BufferSize; i++)
                background_buffer[i] = 1;

            if (_FourierBufferIn != null) _FourierBufferIn.Dispose();
            _FourierBufferIn = UnsafeBuffer.Create(Max_BufferSize, sizeof(Complex));
            FourierBufferIn = (Complex*)_FourierBufferIn;

            if (_FourierBufferOut != null) _FourierBufferOut.Dispose();
            _FourierBufferOut = UnsafeBuffer.Create(Max_BufferSize, sizeof(Complex));
            FourierBufferOut = (Complex*)_FourierBufferOut;

            if (_IntermediateBuffer != null) _IntermediateBuffer.Dispose();
            _IntermediateBuffer = UnsafeBuffer.Create(Max_BufferSize, sizeof(float));
            IntermediateBuffer = (float*)_IntermediateBuffer;


            mFFT.InitBuffers(Max_BufferSize);
        }

        #endregion

        public void UpdateMainBuffer(int BufferSize, bool WindowState)
        {
            Stop();
            Max_BufferSize = BufferSize;
            InitBuffers();
            if (WindowState)
            {
                RestartIFWindow();
                ControlPassiveRadarWindow();
                Start();
            }
        }

        public void Save()
        {
            if (_IFAverageWindow != null && _IFAverageWindow.Visible)
            {
                Stop();
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text|*.txt";
                saveFileDialog1.Title = "Export average data";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName != "")
                {
                    // create a writer and open the file
                    TextWriter tw = new StreamWriter(saveFileDialog1.FileName);
                    string data_out = cumulateIndex * Intermediate_average + "(" + average * Intermediate_average + ")" + "\r\n";
                    for (int i = 0; i < Max_BufferSize; i++)
                    {
                        data_out += (1.0 * (_control.CenterFrequency - _control.RFBandwidth / 2 + 1.0 * i / Max_BufferSize * _control.RFBandwidth) / 1000000).ToString("0.000000000") + "  " + _ExportPtr[i].ToString("0.000000000") + "\r\n";
                    }

                    // write a line of text to the file
                    tw.WriteLine(data_out);

                    // close the stream
                    tw.Close();
                }
                Start();
            }
        }

        public void StopRecording()
        {
            Stop();
        }

        public void StartRecording()
        {
            Start();
        }
    }
}

