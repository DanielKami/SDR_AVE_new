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
using System.Globalization;

namespace SDRSharp.Average
{
    public unsafe class IFProcessor : IIQProcessor
    {
        //Window
        private int cumulateIndex = 0;


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
        public IFAverageWindow _IFAverageWindow;
        System.Diagnostics.Stopwatch watch;
        System.Diagnostics.Stopwatch watch2;//recording time

        public FFT mFFT;
        private float max = 0;
        private readonly SharpEvent _Event = new SharpEvent(false);
        private readonly ComplexFifoStream _iqStream = new ComplexFifoStream(BlockMode.BlockingRead);
        private readonly ISharpControl _control;

        public bool AverageWindowOn = false;

        //Multiple files save
        public bool multiple_save = false;
        private int file_count_number;

        public delegate void MyEventHandler(bool value);
        public static event MyEventHandler Recording;

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
                Flags.window_Width = _IFAverageWindow.Width;
                Flags.window_Height = _IFAverageWindow.Height;
                
                _IFAverageWindow.Dispose();
            }

            _IFAverageWindow = new IFAverageWindow();

            _IFAverageWindow.frequency = _control.CenterFrequency;
            _IFAverageWindow.rate = _control.RFBandwidth;
            _IFAverageWindow.BufferFrameSize = Flags.Max_BufferSize;
            _IFAverageWindow.Data = _PostProcPtr;
            _IFAverageWindow.Gain = Flags.Gain;
            _IFAverageWindow.Level = Flags.Level;

            _IFAverageWindow.Width = Flags.window_Width;
            _IFAverageWindow.Height = Flags.window_Height;

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
                _IFAverageWindow.Gain = Flags.Gain;
                _IFAverageWindow.Level = Flags.Level;
                _IFAverageWindow.file_recording = multiple_save;
                _IFAverageWindow.delayMAX = Flags.Delay;
                _IFAverageWindow.delay = 0;
                _IFAverageWindow.Render();
            }

        }

        public void Reset()
        {
            for (int i = 0; i < Flags.Max_BufferSize * Flags.average_max; i++)
            {
                CumulateBuffer[i] = 0;
            }
            cumulateIndex = 0;

            for (int i = 0; i < Flags.Max_BufferSize; i++)
                background_buffer[i] = 1;
            Flags.background_corrected = false;
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

                while (_control.IsPlaying && _ThreadRunning && total < Flags.Max_BufferSize)
                {
                    var len = Flags.Max_BufferSize - total;
                    //Read reads all available data to len. If len is larger than available data then stop and return new total of read data. After reading len is shorter because we have less data to read                   
                    total += _iqStream.Read(_iqPtr, Flags.Max_BufferSize - len, len);
                }

                #endregion

                #region FFT
                //copy buffer 
                for (int i = 0; i < Flags.Max_BufferSize; i++)
                    FourierBufferIn[i] = _iqPtr[i];

                //calculate FFT
                mFFT.CalcFFT(FourierBufferIn, FourierBufferOut, Flags.Max_BufferSize);
                #endregion
                #region Average
                //Intermediate cumulation FFT buffer
                Flags.Intermediate_cumulateIndex++;

                for (int i = 0; i < Flags.Max_BufferSize; i++)
                    IntermediateBuffer[i] += FourierBufferOut[i].Modulus();
        

                if (Flags.Intermediate_cumulateIndex >= Flags.Intermediate_average)
                {
                    Flags.Intermediate_cumulateIndex = 0;
                    cumulateIndex++;

                    if (cumulateIndex >= Flags.Average)
                    {
                        cumulateIndex = 0;
                        Flags.background_recording = false; //if average buffer is completed stop background aquisition
                                                        
                        if (multiple_save == true)
                        {
                            if (watch != null && watch.ElapsedMilliseconds / 1000 >= Flags.Delay)
                            {
                                HandleMultipleSaving(0);
                                watch.Stop();                             

                                if (watch2 != null) watch2.Stop();
                            }
                            


                        if (watch == null || watch.IsRunning == false)
                                {
                                    watch = System.Diagnostics.Stopwatch.StartNew();
                                }
                        }
                        else
                            watch2 = System.Diagnostics.Stopwatch.StartNew();
                    }

                    //Order buffer data one half on left other on right                    
                    float temp;
                    int hb = Flags.Max_BufferSize / 2;
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
                            pos = Flags.Max_BufferSize - i;
                            temp = IntermediateBuffer[i];
                            IntermediateBuffer[i] = IntermediateBuffer[pos];
                            IntermediateBuffer[pos] = temp;
                        }
                    }
                    temp = 1.0f / Flags.Intermediate_average;
                    for (int i = 0; i < Flags.Max_BufferSize; i++)
                    {
                        IntermediateBuffer[i] *= temp;
                    }

                    if (_control.SquelchEnabled)
                    {
                        max = 0;
                        _IFAverageWindow.ScaleUpdate();
                    }
                    for (int i = 0; i < Flags.Max_BufferSize; i++)
                    {
                        if (IntermediateBuffer[i] > max) max = IntermediateBuffer[i];
                    }

                    //Scale to max
                    int Cm = cumulateIndex * Flags.Max_BufferSize;

                    for (int i = 0; i < Flags.Max_BufferSize; i++)
                        CumulateBuffer[Cm + i] = IntermediateBuffer[i];

                    //Reset intermediate buffer
                    for (int i = 0; i < Flags.Max_BufferSize; i++)
                        IntermediateBuffer[i] = 0;
                }
     
                #endregion
            }

            _iqStream.Flush();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(Flags.background_reset)
            {
                for (int i = 0; i < Flags.Max_BufferSize; i++)
                    background_buffer[i] = 1;
                Flags.background_reset = false;
                Flags.background_corrected = false;
            }

            if (_control.IsPlaying)
            {
                float res;
                float temp = 1.0f / Flags.Average;
                for (int i = 0; i < Flags.Max_BufferSize; i++)
                {
                    res = 0;
                    for (int j = 0; j < Flags.Average; j++)
                        res += CumulateBuffer[j * Flags.Max_BufferSize + i];

                    if (Flags.background_recording == true)
                    {
                        background_buffer[i] = res;
                        _PostProcPtr[i] = Math.Log10(_ExportPtr[i] = res * temp) * Flags.Gain - 35 + Flags.Level;
                    }
                    else
                    {
                        _PostProcPtr[i] = Math.Log10(_ExportPtr[i] = res / background_buffer[i] * temp) * Flags.Gain - 35 + Flags.Level;
                    }
                }

                if (_IFAverageWindow != null && _IFAverageWindow.Visible)
                {
                    _IFAverageWindow.frequency = _control.CenterFrequency;
                    _IFAverageWindow.rate = _control.RFBandwidth;
                    _IFAverageWindow.cumulation = cumulateIndex * Flags.Intermediate_average;
                    _IFAverageWindow.cumulation_max = Flags.Average * Flags.Intermediate_average;
                    _IFAverageWindow.Invalidate();
                    _IFAverageWindow.Gain = Flags.Gain;
                    _IFAverageWindow.Level = Flags.Level;
                    _IFAverageWindow.file_recording = multiple_save;
                    _IFAverageWindow.FileNumber = file_count_number+1;
                    _IFAverageWindow.background_recording = Flags.background_recording;
                    
                    _IFAverageWindow.delayMAX = Flags.Delay;
                    if (watch != null)
                        _IFAverageWindow.delay = Flags.Delay - watch.ElapsedMilliseconds / 1000;
                    else
                        _IFAverageWindow.delay = 0;

                    if (watch2 != null)
                        _IFAverageWindow.recordingTime = watch2.ElapsedMilliseconds / 1000;

                    if (Flags.background_recording)
                        Flags.background_corrected = true;
                    _IFAverageWindow.background_corrected = Flags.background_corrected;
                    _IFAverageWindow.Render();
                }
            }
        }

        private void InitBuffers()
        {
            if (_iqBuffer != null) _iqBuffer.Dispose();
            _iqBuffer = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(Complex));
            _iqPtr = (Complex*)_iqBuffer;

            if (CumulateBuffer_ != null) CumulateBuffer_.Dispose();
            CumulateBuffer_ = UnsafeBuffer.Create(Flags.Max_BufferSize * (Flags.average_max + 1), sizeof(double));
            CumulateBuffer = (float*)CumulateBuffer_;


            // MessageBox.Show("Dot Net Perls is awesome.");
            if (ExportBuffer != null) ExportBuffer.Dispose();
            ExportBuffer = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(double));
            _ExportPtr = (double*)ExportBuffer;

            if (PostProcBuffer != null) PostProcBuffer.Dispose();
            PostProcBuffer = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(double));
            _PostProcPtr = (double*)PostProcBuffer;

            background_buffer = new double[Flags.Max_BufferSize];
            for (int i = 0; i < Flags.Max_BufferSize; i++)
                background_buffer[i] = 1;

            if (_FourierBufferIn != null) _FourierBufferIn.Dispose();
            _FourierBufferIn = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(Complex));
            FourierBufferIn = (Complex*)_FourierBufferIn;

            if (_FourierBufferOut != null) _FourierBufferOut.Dispose();
            _FourierBufferOut = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(Complex));
            FourierBufferOut = (Complex*)_FourierBufferOut;

            if (_IntermediateBuffer != null) _IntermediateBuffer.Dispose();
            _IntermediateBuffer = UnsafeBuffer.Create(Flags.Max_BufferSize, sizeof(float));
            IntermediateBuffer = (float*)_IntermediateBuffer;


            mFFT.InitBuffers(Flags.Max_BufferSize);
        }

        #endregion

        public void UpdateMainBuffer(int BufferSize, bool WindowState)
        {
            Stop();
            Flags.background_corrected = false;
            Flags.Max_BufferSize = BufferSize;
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

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text|*.txt",
                    Title = "Export average data"
                };
                saveFileDialog.ShowDialog();

                //If the file name is not an empty string open it for saving.
                    if (saveFileDialog.FileName != "")
                    {
                        // create a writer and open the file
                        TextWriter tw = new StreamWriter(saveFileDialog.FileName);
                        string data_out = cumulateIndex * Flags.Intermediate_average + "(" + Flags.Average * Flags.Intermediate_average + ")" + "\r\n";
                        for (int i = 0; i < Flags.Max_BufferSize; i++)
                        {
                            data_out += (1.0 * (_control.CenterFrequency - _control.RFBandwidth / 2 + 1.0 * i / Flags.Max_BufferSize * _control.RFBandwidth) / 1000000).ToString("0.000000000") + "  " + _ExportPtr[i].ToString("0.000000000") + "\r\n";
                        }

                        // write a line of text to the file
                        tw.WriteLine(data_out);

                        // close the stream
                        tw.Close();
                    }
                Thread.Sleep(500);
                Start();

            }
        }

        public void SaveMultiple()
        {      
            file_count_number=0;
            multiple_save = true;          
        }

        void HandleMultipleSaving(int value)
        {

            if (multiple_save == true)
                if (file_count_number < Flags.MaxFilesToSave)
                {
                    if (_IFAverageWindow != null && _IFAverageWindow.Visible)
                    {
                        var culture = new CultureInfo("en-US");
                        DateTime localDate = DateTime.Now;

                        string sometext = Flags.Folder + "\\" + Flags.File + "_" + String.Format("{0:0000}", file_count_number+1) + ".txt";
                        //MessageBox.Show(sometext);

                        // create a writer and open the file
                        TextWriter tw = new StreamWriter(sometext);
                        string data_out = localDate.ToString(culture) + "  Counts:" + Flags.Average * Flags.Intermediate_average + "" + "\r\n";
                        for (int index = 0; index < Flags.Max_BufferSize; index++)
                        {
                            data_out += (1.0 * (_control.CenterFrequency - _control.RFBandwidth / 2 + 1.0 * index / Flags.Max_BufferSize * _control.RFBandwidth) / 1000000).ToString("0.000000000") + "  " + _ExportPtr[index].ToString("0.000000000") + "\r\n";
                        }

                        //write a line of text to the file
                        tw.WriteLine(data_out);

                        //close the stream
                        tw.Close();

                        file_count_number++;

                    }
                }
                else
                {
                    Recording(true);
                    multiple_save = false;
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

