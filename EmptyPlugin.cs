/****************************************************************************** 
 * * PROGRAM NAME:  SDR AVE 
 * CLASS:           AveragePlugin 
 * VERSION:         3.x.x 
 * DESCRIPTION:   Advanced Signal Averaging Plugin for #SDR (SDRSharp) 
 * AUTHOR:        Daniel M. Kamiński 
 * LOCATION:      Lublin 2026, Poland 
 * ------------------------------------------------------------------------ 
 * Copyright (c) 2026 Daniel M. Kamiński. All rights reserved. 
 ******************************************************************************/

using SDRSharp.Common;
using SDRSharp.Radio;
using System.ComponentModel;
using System.Windows.Forms;

namespace SDRSharp.Average
{
    public class AveragePlugin : ISharpPlugin, ICanLazyLoadGui, ISupportStatus, IExtendedNameProvider
    {
        private ControlPanel _gui;
        private ISharpControl _control;
        private IFProcessor _processor;
        private IFAverageWindow _averageWindow;

        private bool _isRegistered = false;

        public string DisplayName => "IF Average";
        public string Category => "Data colector";
        public string MenuItemName => DisplayName;
        public bool IsActive => _gui != null && _gui.Visible;

        public UserControl Gui
        {
            get { LoadGui(); return _gui; }
        }

        public void Initialize(ISharpControl control)
        {
            Flags.load();
            _control = control;
            _processor = new IFProcessor(_control);

            // Zabezpieczenie przed wielokrotną rejestracją
            if (!_isRegistered)
            {
                _control.RegisterStreamHook(_processor, ProcessorType.RawIQ);
                _isRegistered = true;
            }

            _control.PropertyChanged += OnControlPropertyChanged;
        }

        private void OnControlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_averageWindow == null || _averageWindow.IsDisposed || !_averageWindow.Visible)
                return;

            if (e.PropertyName == "Frequency" ||
                e.PropertyName == "CenterFrequency" ||
                e.PropertyName == "RFDisplayBandwidth" ||
                e.PropertyName == "InputSampleRate")
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            if (_averageWindow == null || _averageWindow.IsDisposed) return;

            long freq = _control.CenterFrequency != 0 ? _control.CenterFrequency : _control.Frequency;
            int rate = (int)_control.InputSampleRate;

            _averageWindow.CenterFrequency = freq;
            _averageWindow.SampleRate = rate;
            _averageWindow.RefreshGraph();
        }

        public void LoadGui()
        {
            if (_gui == null)
            {
                _gui = new ControlPanel(_control);
                _gui.SetProcessor(_processor);
                _gui.OnOpenRadar += OpenRadarWindow;
                _gui.OnCloseRadar += CloseRadarWindow;
            }
        }

        private void OpenRadarWindow()
        {
            if (_averageWindow == null || _averageWindow.IsDisposed)
            {
                _averageWindow = new IFAverageWindow();
                _processor._IFAverageWindow = _averageWindow;

                // Zapobiegamy całkowitemu zamknięciu okna
                _averageWindow.FormClosing += (s, e) =>
                {
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        e.Cancel = true;
                        _averageWindow.Hide();
                        _processor.Enabled = false;

                        if (_gui != null)
                            _gui.UpdateEnableCheckbox(false);
                    }
                };
            }

            UpdateData();
            _processor.Enabled = true;
            _processor.ClearBuffers();           // ważne - reset przy każdym otwarciu
            _averageWindow.Show();
        }

        private void CloseRadarWindow()
        {
            if (_averageWindow != null)
                _averageWindow.Hide();

            _processor.Enabled = false;
        }

        public void Close()
        {
            if (_control != null)
            {
                _control.PropertyChanged -= OnControlPropertyChanged;

                if (_isRegistered)
                {
                    _control.UnregisterStreamHook(_processor);
                    _isRegistered = false;
                }
            }

            if (_averageWindow != null && !_averageWindow.IsDisposed)
                _averageWindow.Dispose();

            Flags.save();
        }

        public string Status => _processor != null && _processor.Enabled ? "Running" : "Idle";
        public string ExtendedName => DisplayName;
    }
}