/****************************************************************************** 
 * * PROGRAM NAME:  SDR AVE 
 * CLASS:           IFProcessor 
 * VERSION:         3.x.x 
 * DESCRIPTION:   Advanced Signal Averaging Plugin for #SDR (SDRSharp) 
 * AUTHOR:        Daniel M. Kamiński 
 * LOCATION:      Lublin 2026, Poland 
 * ------------------------------------------------------------------------ 
 * Copyright (c) 2026 Daniel M. Kamiński. All rights reserved. 
 ******************************************************************************/

using SDRSharp.Common;
using SDRSharp.Radio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRSharp.Average
{
    public unsafe class IFProcessor : IIQProcessor
    {
        private DateTime _lastSaveTime = DateTime.MinValue;

        public delegate void MyEventHandler(bool value);
        public static event MyEventHandler Recording;

        private ISharpControl _control;
        public IFAverageWindow _IFAverageWindow;
        private readonly object _lock = new object();
        private volatile bool _isReconfiguring;


        public bool Enabled { get; set; }
        public double SampleRate { get; set; }

        private UnsafeBuffer _PostProcBuffer;
        private double* _PostProcPtr;
        private UnsafeBuffer _FFTWorkBuffer;
        private Complex* _FFTWorkPtr;
        private UnsafeBuffer ExportBuffer;
        private double* _ExportPtr;

        private double[] background_buffer;
        private int _avgCount = 0;
        private double[] _intermediateSumBuffer;
        private int _intermediateCounter = 0;

        // ZAMIENIAMY jagged na flat + prosty ring
        private double[] _frameHistoryFlat;     // rozmiar: Average * len
        private double[] _sumBuffer;
        private double[] _movingAverageBuffer;
        private int _ringSize = 0;
        private int _historyWriteIndex = 0;
        private int _currentHistoryCount = 0;

        // Median – ręczny, bez Sort
        private double[][] _medianHistory;      // zostawiamy na razie (można później spłaszczyć)
        private double[] _medianSortBuffer;     // niepotrzebne przy ręcznym medianie
        private int _medianBufferSize = 5;
        private int _medianIndex = 0;

        private double[] _currentFrame;         // prealokowana

        private DateTime _lastProcessTime = DateTime.MinValue;


        private double[] _rawPowerFrame;        // po FFT, przed jakimkolwiek uśrednianiem

        public FFT mFFT;


        // Savitzky-Golay coefficients
        private static readonly double[] SG_Coeffs = { -3.0 / 35.0, 12.0 / 35.0, 17.0 / 35.0, 12.0 / 35.0, -3.0 / 35.0 };


        // Kolejka do przekazywania danych między wątkami
        private BlockingCollection<Complex[]> _dataQueue = new BlockingCollection<Complex[]>(new ConcurrentStack<Complex[]>(), 2);
        private Thread _workerThread;
        private bool _running;


        double ave_estimated_time;
        private List<double> _etaHistory = new List<double>();
        private double _stableMeasuredTime;

        private DateTime _lastFpsUpdate = DateTime.MinValue;
        private int _fftFramesThisSecond = 0;
        private double _currentFps = 0.0;

        // === STATYSTYKI WEJŚCIA SDR# ===
        private DateTime _lastInputStatTime = DateTime.MinValue;
        private long _totalProcessCalls = 0;
        private long _totalSamplesReceived = 0;

        // ===================================================================
        // Constructor
        // ===================================================================
        public IFProcessor(ISharpControl control)
        {
            _control = control;
            mFFT = new FFT();
            SetBuffers(Flags.Max_BufferSize);
            Reset();
            ClearBuffers();

            // URUCHAMIAMY OSOBNY WĄTEK OBLICZENIOWY
            _running = true;
            _workerThread = new Thread(CalculationLoop) { IsBackground = true, Priority = ThreadPriority.Highest };
            _workerThread.Start();


        }

        // ===================================================================
        // SetBuffers – alokujemy stały ring buffer RAZ
        // ===================================================================
        // ===================================================================
        // SetBuffers – zoptymalizowana wersja pod wydajność
        // ===================================================================
        public void SetBuffers(int size)
        {
            lock (_lock)
            {
                _isReconfiguring = true;
                try
                {
                    Flags.Max_BufferSize = size;
                    mFFT.InitBuffers(size);
                    mFFT.PrepareFFT();

                    _rawPowerFrame = new double[size];

                    // Buffery UnsafeBuffer
                    if (_PostProcBuffer != null) _PostProcBuffer.Dispose();
                    _PostProcBuffer = UnsafeBuffer.Create(size, sizeof(double));
                    _PostProcPtr = (double*)_PostProcBuffer;

                    if (_FFTWorkBuffer != null) _FFTWorkBuffer.Dispose();
                    _FFTWorkBuffer = UnsafeBuffer.Create(size, sizeof(Complex));
                    _FFTWorkPtr = (Complex*)_FFTWorkBuffer;

                    if (ExportBuffer != null) ExportBuffer.Dispose();
                    ExportBuffer = UnsafeBuffer.Create(size, sizeof(double));
                    _ExportPtr = (double*)ExportBuffer;

                    // Podstawowe bufory
                    _intermediateSumBuffer = new double[size];
                    background_buffer = new double[size];
                    _currentFrame = new double[size];

                    _avgCount = 0;
                    _intermediateCounter = 0;

                    // SumBuffer i MovingAverageBuffer (używane przy ring bufferze)
                    if (_sumBuffer == null || _sumBuffer.Length != size)
                    {
                        _sumBuffer = new double[size];
                        _movingAverageBuffer = new double[size];
                    }

                    // Reset ring buffera – wymusimy ponowną alokację przy pierwszej zmianie Average
                    _ringSize = 0;
                    _historyWriteIndex = 0;
                    _currentHistoryCount = 0;

                    // Median filter buffers
                    if (_medianHistory == null || _medianHistory.Length != _medianBufferSize ||
                        (_medianHistory.Length > 0 && _medianHistory[0].Length != size))
                    {
                        _medianHistory = new double[_medianBufferSize][];
                        for (int i = 0; i < _medianBufferSize; i++)
                            _medianHistory[i] = new double[size];
                    }
                    else
                    {
                        // Tylko czyszczenie, jeśli rozmiar się zgadza
                        for (int i = 0; i < _medianBufferSize; i++)
                            Array.Clear(_medianHistory[i], 0, size);
                    }

                    _medianIndex = 0;

                    // Wyczyść główne bufory
                    Array.Clear(_intermediateSumBuffer, 0, size);
                    Array.Clear(_sumBuffer, 0, size);
                    Array.Clear(_movingAverageBuffer, 0, size);
                    Array.Clear(_currentFrame, 0, size);

                    //  _isFirstFrame = true;
                }
                finally
                {
                    _isReconfiguring = false;
                }
            }
        }

        // ===================================================================
        // UpdateMovingAverage – prawdziwa średnia z ostatnich N ramek
        // ===================================================================
        private void UpdateMovingAverage(double[] newFrame, int len)
        {
            int avg = Math.Max(1, Flags.Average);

            // Re-alokacja tylko gdy zmieni się rozmiar Average
            if (_ringSize != avg)
            {
                _frameHistoryFlat = new double[avg * len];
                Array.Clear(_sumBuffer, 0, len);
                Array.Clear(_movingAverageBuffer, 0, len);

                _ringSize = avg;
                _historyWriteIndex = 0;
                _currentHistoryCount = 0;
            }

            int offset = _historyWriteIndex * len;   // start bieżącej ramki w flat array

            bool isFull = (_currentHistoryCount >= avg);

            for (int i = 0; i < len; i++)
            {
                if (isFull)
                    _sumBuffer[i] -= _frameHistoryFlat[offset + i];

                _sumBuffer[i] += newFrame[i];
                _frameHistoryFlat[offset + i] = newFrame[i];
            }

            _historyWriteIndex = (_historyWriteIndex + 1) % _ringSize;

            if (!isFull)
                _currentHistoryCount++;

            double divisor = _currentHistoryCount;
            for (int i = 0; i < len; i++)
                _movingAverageBuffer[i] = _sumBuffer[i] / divisor;
        }

        public void Process(Complex* buffer_, int length)
        {
            if (!Enabled || _isReconfiguring || !_running)
                return;

            // ==================== POMIAR WEJŚCIA ====================
            _totalProcessCalls++;
            _totalSamplesReceived += length;

            if ((DateTime.Now - _lastInputStatTime).TotalSeconds >= 1.0)
            {
                double seconds = (DateTime.Now - _lastInputStatTime).TotalSeconds;

                double callsPerSec = _totalProcessCalls / seconds;
                double samplesPerSec = _totalSamplesReceived / seconds;

                Flags.SamplesPerSecond = samplesPerSec;           // dodaj do Flags
                Flags.ProcessCallsPerSecond = callsPerSec;        // dodaj do Flags

                //Console.WriteLine($"[INPUT] Process() calls/s: {callsPerSec:F1} | " +
                //                  $"Samples/s: {samplesPerSec:F0} | " +
                //                  $"Avg block size: {(samplesPerSec / callsPerSec):F0} | " +
                //                  $"Last block: {length}");

                _totalProcessCalls = 0;
                _totalSamplesReceived = 0;
                _lastInputStatTime = DateTime.Now;
            }
            // =======================================================

            // Tworzymy kopię bufora i wrzucamy do kolejki
            Complex[] block = new Complex[length];
            fixed (Complex* pDest = block)
            {
                Buffer.MemoryCopy(buffer_, pDest, length * sizeof(Complex), length * sizeof(Complex));
            }

            _dataQueue.TryAdd(block);
        }
        private void CalculationLoop()
        {
            while (_running)
            {
                // Ochrona przed zamkniętą/completed kolejką
                if (_dataQueue.IsCompleted)
                {
                    Thread.Sleep(20);
                    continue;
                }

                if (_dataQueue.TryTake(out Complex[] block, 100))   // timeout 100ms
                {
                    if (block == null || block.Length == 0)
                        continue;

                    int fftSize = Flags.Max_BufferSize;
                    int totalLength = block.Length;
                    int offset = 0;
                    int fullFFTs = 0;

                    fixed (Complex* pBlock = block)
                    {
                        while (offset + fftSize <= totalLength)
                        {
                            Process_(pBlock + offset, fftSize);
                            offset += fftSize;
                            fullFFTs++;
                        }
                    }

                    // Aktualizacja statystyk w Flags
                    Flags.LastBlockSizeReceived = totalLength;
                    Flags.LastBlockFullFFTs = fullFFTs;
                    Flags.LastBlockDiscardedSamples = totalLength - (fullFFTs * fftSize);

                    // Log co ~5 sekund (żeby nie spamować konsoli)
                    if (fullFFTs > 0 && (DateTime.Now.Second % 5 == 0))
                    {
                        Console.WriteLine($"[BUF] Received: {totalLength,5} samples | " +
                                          $"FFTs: {fullFFTs} | " +
                                          $"Discarded: {Flags.LastBlockDiscardedSamples}");
                    }
                }
            }
        }

        // ===================================================================
        // SingleProcess – POPRAWIONA WERSJA (reset PO zapisie)
        // ===================================================================
        private void Process_(Complex* buffer_, int length)
        {
            if (!Enabled || _isReconfiguring || _IFAverageWindow == null || _IFAverageWindow.IsDisposed)
                return;

            if (!Flags.start_stop) return;

            int len = Flags.Max_BufferSize;

            // 🔴 GLOBALNA BLOKADA DELAY – NA SAMYM POCZĄTKU
            if (Flags.multiple_save && _lastSaveTime != DateTime.MinValue)
            {
                double elapsed = (DateTime.Now - _lastSaveTime).TotalSeconds;

                if (elapsed < Flags.Delay)
                {
                    // tylko aktualizacja UI (countdown)
                    if (_IFAverageWindow != null && !_IFAverageWindow.IsDisposed)
                        _IFAverageWindow.LastSaveTimeForDisplay = _lastSaveTime;
                    _IFAverageWindow.RefreshGraph();
                    return; // 🔴 HARD STOP – brak akwizycji i FFT
                }
                else
                {
                    // koniec delay → start nowej akwizycji
                    _lastSaveTime = DateTime.MinValue;
                    ResetAccumulator();
                    _IFAverageWindow.RefreshGraph();
                    return; // 🔴 bardzo ważne – nie używamy starej ramki
                }
            }

            // Kopia bufora
            Complex* buffer = stackalloc Complex[len];
            Buffer.MemoryCopy(buffer_, buffer, len * sizeof(Complex), len * sizeof(Complex));

            DateTime frameStart = DateTime.Now;

            const double epsilon = 1e-20;

            mFFT.ProcessFFT(buffer, _FFTWorkPtr);

            // 1. Surowa moc
            fixed (double* pRaw = _rawPowerFrame)
            {
                Complex* src = _FFTWorkPtr;
                double* dest = pRaw;
                for (int i = 0; i < len; i++)
                {
                    double r = src[i].Real;
                    double im = src[i].Imag;
                    *dest++ = r * r + im * im;
                }
            }

            // 2. Median / sumowanie
            if (Flags.boolUseMedianFilter)
            {
                for (int i = 0; i < len; i++)
                {
                    double a = _medianHistory[0][i];
                    double b = _medianHistory[1][i];
                    double c = _medianHistory[2][i];
                    double d = _medianHistory[3][i];
                    double e = _medianHistory[4][i];

                    _medianHistory[_medianIndex][i] = _rawPowerFrame[i];

                    double medianVal = Median5(a, b, c, d, e);

                    if (i == len / 2 && _rawPowerFrame[i] > medianVal * 10.0)
                        Flags.RejectedFramesCount++;

                    _intermediateSumBuffer[i] += medianVal;
                }

                _medianIndex = (_medianIndex + 1) % _medianBufferSize;
            }
            else
            {
                fixed (double* pRaw = _rawPowerFrame)
                fixed (double* pSum = _intermediateSumBuffer)
                {
                    double* src = pRaw;
                    double* dest = pSum;
                    for (int i = 0; i < len; i++)
                        *dest++ += *src++;
                }
            }

            _intermediateCounter++;

            // 3. Intermediate average
            if (_intermediateCounter >= Math.Max(1, Flags.Intermediate_average))
            {
                double fftNorm = 1.0 / ((double)len);

                for (int i = 0; i < len; i++)
                {
                    _currentFrame[i] = Math.Sqrt((_intermediateSumBuffer[i] / _intermediateCounter) / len);
                }

                //Stage2
                UpdateMovingAverage(_currentFrame, len);

                if (_avgCount < Flags.Average) _avgCount++;

                // === ZAMIANA Parallel.For na zwykłą pętlę przy małych rozmiarach ===
                if (len <= 256)
                {
                    for (int i = 0; i < len; i++)
                    {
                        ProcessBin(i);
                    }
                }
                else
                {
                    Parallel.For(0, len, i => ProcessBin(i));
                }

                // Background zakończenie
                if (Flags.background_recording && _avgCount >= Flags.Average)
                {
                    Flags.background_recording = false;
                    Flags.background_recorded = true;
                    Flags.BackgroundFileName = "Internal Buffer";
                    Array.Copy(background_buffer, _movingAverageBuffer, len);
                    _avgCount = 0;
                    Console.Beep(1000, 200);
                }

                // === MULTIPLE SAVE ===
                if (Flags.multiple_save && _avgCount >= Flags.Average)
                {
                    if (Flags.file_count_number >= Flags.MaxFilesToSave)
                    {
                        Flags.multiple_save = false;
                        return;
                    }

                    string filename = Path.Combine(Flags.Folder, $"{Flags.File}_{Flags.file_count_number + 1:0000}.txt");
                    DataSave(filename);
                    Flags.file_count_number++;

                    // START DELAY
                    _lastSaveTime = DateTime.Now;

                    if (_IFAverageWindow != null && !_IFAverageWindow.IsDisposed)
                        _IFAverageWindow.LastSaveTimeForDisplay = _lastSaveTime;

                    return;
                }

                if (Flags.UseSavitzkyGolay)
                    ApplySavitzkyGolay(_PostProcPtr, len);

                DateTime now = DateTime.Now;
                if (_lastProcessTime != DateTime.MinValue)
                {
                    double currentFrameInterval = (now - _lastProcessTime).TotalMilliseconds;
                    ave_estimated_time = (0.90 * ave_estimated_time) + (0.1 * currentFrameInterval);

                    _etaHistory.Add(ave_estimated_time);
                    if (_etaHistory.Count > 20) _etaHistory.RemoveAt(0); // Bufor 20 próbek

                    // Wyznaczamy medianę do wyświetlenia
                    var sorted = _etaHistory.OrderBy(x => x).ToList();
                    _stableMeasuredTime = sorted[sorted.Count / 2];
                }

                _lastProcessTime = now;
 
                // UI update
                _IFAverageWindow.CurrentSampleCount = _avgCount;
                _IFAverageWindow.TargetSampleCount = Flags.Average;
                _IFAverageWindow.Data = _PostProcPtr;
                _IFAverageWindow.BufferFrameSize = len;
                _IFAverageWindow.RefreshGraph();
                _IFAverageWindow.MeasuredFrameTime = _stableMeasuredTime;

                Array.Clear(_intermediateSumBuffer, 0, len);
                _intermediateCounter = 0;
            }
 
        }

        private void ProcessBin(int i)
        {
            double displayVal = _movingAverageBuffer[i];

            _ExportPtr[i] = displayVal;

            if (Flags.background_recording)
                background_buffer[i] = Math.Max(displayVal, 1e-20);

            double dB_background = Flags.background_recorded && !Flags.background_recording
                ? 20.0 * Math.Log10(Math.Max(background_buffer[i], 1e-20))
                : 0.0;

            double final_dB = 20.0 * Math.Log10(displayVal + 1e-20) - dB_background;

            _PostProcPtr[i] = (float)(final_dB + Flags.SignalOffset);
        }

        private static double Median5(double a, double b, double c, double d, double e)
        {
            // Prosty, bardzo szybki median dla 5 wartości
            if (a > b) { double t = a; a = b; b = t; }
            if (c > d) { double t = c; c = d; d = t; }
            if (a > c) { double t = a; a = c; c = t; }
            if (b > d) { double t = b; b = d; d = t; }
            if (b > c) { double t = b; b = c; c = t; }

            double m1 = Math.Max(a, Math.Min(b, e));
            return Math.Max(m1, Math.Min(c, Math.Max(d, e))); // uproszczone – działa dobrze dla mediany
        }

        // ===================================================================
        // ClearBuffers / ResetAccumulator / Reset – poprawione
        // ===================================================================
        public void ClearBuffers()
        {
            lock (_lock)
            {
                if (_intermediateSumBuffer != null) Array.Clear(_intermediateSumBuffer, 0, _intermediateSumBuffer.Length);
                if (_sumBuffer != null) Array.Clear(_sumBuffer, 0, _sumBuffer.Length);
                if (_movingAverageBuffer != null) Array.Clear(_movingAverageBuffer, 0, _movingAverageBuffer.Length);
                if (_rawPowerFrame != null) Array.Clear(_rawPowerFrame, 0, _rawPowerFrame.Length);
                _intermediateCounter = 0;
                _avgCount = 0;
                _historyWriteIndex = 0;
                _currentHistoryCount = 0;

                ave_estimated_time = 1.025 * Flags.Intermediate_average / 10;
            }
        }

        public void ResetAccumulator()
        {
            lock (_lock)
            {
                _avgCount = 0;
                _intermediateCounter = 0;
                _historyWriteIndex = 0;
                _currentHistoryCount = 0;


                if (_intermediateSumBuffer != null) Array.Clear(_intermediateSumBuffer, 0, _intermediateSumBuffer.Length);
                if (_sumBuffer != null) Array.Clear(_sumBuffer, 0, _sumBuffer.Length);
                if (_movingAverageBuffer != null) Array.Clear(_movingAverageBuffer, 0, _movingAverageBuffer.Length);
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                ResetAccumulator();


                Flags.RejectedFramesCount = 0;
                Flags.multiple_save = false;
                Flags.single_save_active = false;
                Flags.file_count_number = 0;

                if (_medianHistory != null)
                    for (int i = 0; i < _medianBufferSize; i++)
                        Array.Clear(_medianHistory[i], 0, _medianHistory[i].Length);


                ClearBuffers();
                Flags.background_reset = true;
                Console.Beep(500, 200);
            }
        }

        // ===================================================================
        // Reszta metod bez zmian (Save, BackgroundRead, BackgroundSave itd.)
        // ===================================================================
        private System.Windows.Forms.Timer _singleSaveTimer;


        /// <summary>
        /// Loads background (noise floor) from a text file.
        /// The file must contain exactly the same number of data lines as the current FFT size.
        /// </summary>
        /// <summary>
        /// Loads background (noise floor) from a text file.
        /// Automatically handles extra empty lines at the end of the file.
        /// </summary>
        public void BackgroundRead()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Text files|*.txt",
                Title = "Load Background File"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string[] lines = File.ReadAllLines(ofd.FileName);

                if (lines.Length < 2)
                {
                    MessageBox.Show("The background file is empty or corrupted.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // === Poprawne liczenie linii danych (pomijamy nagłówek i puste linie na końcu) ===
                int fileDataSize = 0;
                for (int i = 1; i < lines.Length; i++)   // zaczynamy od linii 1 (pomijamy nagłówek)
                {
                    string trimmed = lines[i].Trim();
                    if (!string.IsNullOrWhiteSpace(trimmed))
                    {
                        fileDataSize++;
                    }
                }

                lock (_lock)
                {
                    // Check for size mismatch
                    if (fileDataSize != Flags.Max_BufferSize)
                    {
                        var result = MessageBox.Show(
                            $"Size mismatch!\n\n" +
                            $"Current FFT size : {Flags.Max_BufferSize} bins\n" +
                            $"Background file  : {fileDataSize} bins\n\n" +
                            "Do you want to change FFT size to match the background file?",
                            "Buffer Size Mismatch",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            SetBuffers(fileDataSize);
                        }
                        else
                        {
                            MessageBox.Show("Loading cancelled due to size mismatch.", "Cancelled",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    var culture = CultureInfo.InvariantCulture;
                    int dataIndex = 0;

                    // Wczytujemy dane – pomijamy nagłówek i puste linie
                    for (int i = 1; i < lines.Length && dataIndex < Flags.Max_BufferSize; i++)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 2)
                        {
                            background_buffer[dataIndex] = double.Parse(parts[1], culture);
                            dataIndex++;
                        }
                        else
                        {
                            background_buffer[dataIndex] = 1e-9;
                            dataIndex++;
                        }
                    }

                    // Wypełniamy resztę bufora bezpieczną wartością (jeśli plik miał mniej linii)
                    while (dataIndex < Flags.Max_BufferSize)
                    {
                        background_buffer[dataIndex] = 1e-9;
                        dataIndex++;
                    }

                    Flags.background_recorded = true;
                    Flags.BackgroundFileName = Path.GetFileName(ofd.FileName);
                    _avgCount = 0;
                    Flags.background_reset = true;
                }

                Console.Beep(1500, 100);
                MessageBox.Show($"Background loaded successfully!\n" +
                                $"File : {Flags.BackgroundFileName}\n" +
                                $"Size : {Flags.Max_BufferSize} bins",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading background file:\n\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveBackground()
        {
            if (background_buffer == null || !Flags.background_recorded) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text|*.txt",
                FileName = "background.txt"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var culture = new CultureInfo("en-US");
                using (TextWriter tw = new StreamWriter(sfd.FileName))
                {
                    int bufferSize = Flags.Max_BufferSize;

                    tw.WriteLine($"{DateTime.Now.ToString(culture)}  Counts:{_avgCount * Flags.Intermediate_average}");

                    for (int i = 0; i < bufferSize; i++)
                    {
                        double freq = (_control.CenterFrequency - _control.RFBandwidth / 2.0) +
                                      (double)i / bufferSize * _control.RFBandwidth;

                        tw.WriteLine($"{(freq / 1_000_000.0).ToString("F9", culture)}  {background_buffer[i].ToString("F9", culture)}");
                    }
                    tw.WriteLine("");
                }

                Flags.BackgroundFileName = Path.GetFileName(sfd.FileName);
                Flags.background_save_active = true;
                Console.Beep(2000, 50);

                System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => Flags.background_save_active = false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu background:\n" + ex.Message);
            }
        }

        public void SaveMultiple()
        {
            lock (_lock)
            {
                Flags.file_count_number = 0;
                Flags.multiple_save = true;

                _lastSaveTime = DateTime.MinValue; // 🔴 brak delay na starcie

                if (_IFAverageWindow != null && !_IFAverageWindow.IsDisposed)
                    _IFAverageWindow.LastSaveTimeForDisplay = DateTime.MinValue;

                ResetAccumulator();
            }

            Console.Beep(500, 200);
        }

        public void Save()
        {
            if (_IFAverageWindow == null || _IFAverageWindow.IsDisposed) return;
            var culture = CultureInfo.InvariantCulture;
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text|*.txt", FileName = "Data.txt" };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DataSave(sfd.FileName);
                if (_singleSaveTimer == null)
                {
                    _singleSaveTimer = new System.Windows.Forms.Timer { Interval = 3000 };
                    _singleSaveTimer.Tick += (s, e) => { Flags.single_save_active = false; _singleSaveTimer.Stop(); };
                }
                _singleSaveTimer.Stop(); _singleSaveTimer.Start();
            }
        }



        void DataSave(string FileName)
        {
            var culture = new CultureInfo("en-US");
            using (TextWriter tw = new StreamWriter(FileName))
            {
                //   double totalDuration = _averageFrameDuration * Flags.Average;
                tw.WriteLine($"{DateTime.Now.ToString(culture)}  Counts:{_avgCount * Flags.Intermediate_average}");

                int bufferSize = Flags.Max_BufferSize;   // ZAWSZE używaj aktualnego rozmiaru!

                for (int i = 0; i < bufferSize; i++)
                {
                    double freq = (_control.CenterFrequency - _control.RFBandwidth / 2.0) +
                                  (double)i / bufferSize * _control.RFBandwidth;

                    double ds = Flags.background_recorded
                        ? _ExportPtr[i] / background_buffer[i]
                        : _ExportPtr[i];

                    tw.WriteLine($"{(freq / 1000000.0).ToString("F9", culture)}  {ds.ToString("F9", culture)}");
                }
                tw.WriteLine("");
            }

            Flags.LastSavedFileName = Path.GetFileName(FileName);
            Flags.single_save_active = true;
            if (Flags.beep_active)
                Console.Beep(2000, 50);
        }

        public void ResetBackground()
        {
            lock (_lock)
            {
                Flags.background_recorded = false;
                Flags.background_recording = false;
                Flags.BackgroundFileName = "";
                _avgCount = 0;
                if (background_buffer != null) Array.Clear(background_buffer, 0, background_buffer.Length);
                Flags.background_reset = true;
            }
        }

        private void ApplySavitzkyGolay(double* data, int len)
        {
            double[] temp = new double[len];
            for (int i = 2; i < len - 2; i++)
            {
                temp[i] = (data[i - 2] * SG_Coeffs[0]) + (data[i - 1] * SG_Coeffs[1]) +
                          (data[i] * SG_Coeffs[2]) + (data[i + 1] * SG_Coeffs[3]) +
                          (data[i + 2] * SG_Coeffs[4]);
            }
            for (int i = 2; i < len - 2; i++)
                data[i] = temp[i];
        }

    }


}