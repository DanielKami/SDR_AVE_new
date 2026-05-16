
/******************************************************************************
 * * PROGRAM NAME:  SDR AVE
 * CLASS:           ControlPanel
 * VERSION:         3.x.x
 * * DESCRIPTION:   Advanced Signal Averaging Plugin for #SDR (SDRSharp)
 * Enhances visual representation of the IF spectrum.
 * * AUTHOR:        Daniel M. Kamiński
 * LOCATION:      Lublin 2026, Poland
 * * TARGET REFS:   .NET 9.0 | .NET 8.0
 * COMPATIBILITY: Designed for #SDR by Youssef Touil
 * * ------------------------------------------------------------------------
 * Copyright (c) 2026 Daniel M. Kamiński. All rights reserved.
 ******************************************************************************/

using SDRSharp.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRSharp.Average
{
    public partial class ControlPanel : UserControl
    {
        private ISharpControl _control;
        private IFProcessor _processor;

        // EVENTS FOR WINDOW HANDLING
        public event Action OnOpenRadar;
        public event Action OnCloseRadar;

        /// <summary>
        /// Name: ControlPanel (Constructor)
        /// Input: ISharpControl control
        /// Output: None
        /// Description: Initializes the control panel, sets default UI values from Flags, and handles initial state.
        /// </summary>
        public ControlPanel(ISharpControl control)
        {
            _control = control;
            InitializeComponent();

            IFProcessor.Recording += new IFProcessor.MyEventHandler(HandleButtonOn);

            label2.Text = "v." + Flags.version;

            // 1. Set sliders to values from Flags (loaded from file or defaults)
            // Safely setting sliders using Math.Clamp
            trackBarGain.Value = Math.Clamp((int)(Flags.Gain * 100), trackBarGain.Minimum, trackBarGain.Maximum);
            trackBarLevel.Value = Math.Clamp((int)Flags.Level, trackBarLevel.Minimum, trackBarLevel.Maximum);
            trackBarAverage.Value = Math.Clamp(Flags.Average, trackBarAverage.Minimum, trackBarAverage.Maximum);

            // Set BufferFrameSize (comboBox1)
            if (comboBox1.Items.Contains(Flags.Max_BufferSize.ToString()))
            {
                comboBox1.SelectedItem = Flags.Max_BufferSize.ToString();
            }
            else
            {
                // If not in the list, set default
                comboBox1.SelectedIndex = 0;
            }

            // Set Intermediate_average (comboBox2)
            if (comboBox2.Items.Contains(Flags.Intermediate_average.ToString()))
            {
                comboBox2.SelectedItem = Flags.Intermediate_average.ToString();
            }
            else
            {
                comboBox2.SelectedIndex = 0;
            }

            // 2. Set label texts next to sliders
            textBoxGain.Text = Flags.Gain.ToString();
            textBoxLevel.Text = Flags.Level.ToString();
            textBoxAverage.Text = (Flags.Average * Flags.Intermediate_average).ToString();

            // Safe setting for NumericUpDown values
            numericUpDown1.Value = Math.Clamp((decimal)Flags.MaxFilesToSave, numericUpDown1.Minimum, numericUpDown1.Maximum);
            numericUpDown2.Value = Math.Clamp((decimal)Flags.Delay, numericUpDown2.Minimum, numericUpDown2.Maximum);

            // SignalOffset handling (assuming negative value inverted to positive for display)
            decimal offsetValue = (decimal)(-Flags.SignalOffset);
            numericUpDown3.Value = Math.Clamp(offsetValue, numericUpDown3.Minimum, numericUpDown3.Maximum);

            checkBoxBeep.Checked = Flags.beep_active;
            checkBoxStat.Checked = Flags.statistics;

        }

        /// <summary>
        /// Name: SetProcessor
        /// Input: IFProcessor processor
        /// Output: None
        /// Description: Associates the UI with the backend IFProcessor instance.
        /// </summary>
        public void SetProcessor(IFProcessor processor)
        {
            _processor = processor;
        }

        /// <summary>
        /// Name: UpdateEnableCheckbox
        /// Input: bool isChecked
        /// Output: None
        /// Description: Thread-safe update of the Radar checkbox without triggering unwanted change events.
        /// </summary>
        public void UpdateEnableCheckbox(bool isChecked)
        {
            if (enablePassiveRadarWindow.InvokeRequired)
            {
                enablePassiveRadarWindow.Invoke(new Action(() => UpdateEnableCheckbox(isChecked)));
                return;
            }

            // Unsubscribe event temporarily so the manual change doesn't trigger OnOpen/Close logic
            enablePassiveRadarWindow.CheckedChanged -= enablePassiveRadarWindow_CheckedChanged_2;
            enablePassiveRadarWindow.Checked = isChecked;
            enablePassiveRadarWindow.CheckedChanged += enablePassiveRadarWindow_CheckedChanged_2;
        }

        /// <summary>
        /// Name: enablePassiveRadarWindow_CheckedChanged
        /// Input: object sender, EventArgs e
        /// Output: None (async)
        /// Description: Handles the UI trigger to open or close the Passive Radar window.
        /// </summary>
        private async void enablePassiveRadarWindow_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Delay(500);

            if (enablePassiveRadarWindow.Checked)
            {
                OnOpenRadar?.Invoke();
            }
            else
            {
                OnCloseRadar?.Invoke();
            }
        }

        /// <summary>
        /// Name: trackBarGain_ValueChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates signal gain and resets background averaging to show immediate effects.
        /// </summary>
        private void trackBarGain_ValueChanged(object sender, EventArgs e)
        {
            Flags.Gain = 0.01f * trackBarGain.Value;
            textBoxGain.Text = Flags.Gain.ToString();
            Flags.background_reset = true;
        }

        /// <summary>
        /// Name: trackBarAverage_ValueChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates averaging settings and triggers a processor background reset.
        /// </summary>
        private void trackBarAverage_ValueChanged(object sender, EventArgs e)
        {
            Flags.Average = trackBarAverage.Value;
            textBoxAverage.Text = (Flags.Average * Flags.Intermediate_average).ToString();
            Flags.background_reset = true;
        }

        /// <summary>
        /// Name: button1_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Triggers a reset of the data within the processor.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            _processor?.Reset();
        }

        /// <summary>
        /// Name: button2_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Triggers a single data save operation.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            _processor?.Save();
        }

        /// <summary>
        /// Name: comboBox1_SelectedIndexChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Safely updates buffer sizes by pausing processing, reapplying setup, and resuming.
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || _processor == null) return;

            int newBufferSize = int.Parse(comboBox1.Text);

            // Store current state and stop processor
            bool wasEnabled = _processor.Enabled;
            _processor.Enabled = false;

            // Perform safe buffer setup
            _processor.SetBuffers(newBufferSize);

            // Restore previous state
            _processor.Enabled = wasEnabled;
        }

        /// <summary>
        /// Name: comboBox2_SelectedIndexChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates intermediate averaging and clears current processor buffers.
        /// </summary>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_processor == null) return;

            if (int.TryParse(comboBox2.Text, out int result))
            {
                Flags.Intermediate_average = result;
                _processor.ClearBuffers();
            }
        }

        private void label7_Click(object sender, EventArgs e) { }

        private void trackBarGain_Scroll(object sender, EventArgs e) { }

        /// <summary>
        /// Name: trackBarLevel_ValueChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates the signal level flag and UI text.
        /// </summary>
        private void trackBarLevel_ValueChanged(object sender, EventArgs e)
        {
            Flags.Level = trackBarLevel.Value;
            textBoxLevel.Text = Flags.Level.ToString();
        }

        /// <summary>
        /// Name: button3_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Toggles the global start/stop flag.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            Flags.start_stop = !Flags.start_stop;
        }

        /// <summary>
        /// Name: button_background_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Full reset and start of background recording process.
        /// </summary>
        private void button_background_Click(object sender, EventArgs e)
        {
            Flags.background_reset = true;
            _processor?.ResetBackground();
            _processor?.Reset();
            _processor.ClearBuffers();
            Flags.background_recording = true;
            Flags.background_recorded = false;
        }

        private void enablePassiveRadarWindow_CheckedChanged_1(object sender, EventArgs e) { }

        /// <summary>
        /// Name: button3_Click_1
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Initiates sequential (multiple) file saving.
        /// </summary>
        private void button3_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = false;
            _processor?.SaveMultiple();
        }

        private void trackBarAverage_Scroll(object sender, EventArgs e) { }

        /// <summary>
        /// Name: button4_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Stops the sequential saving process.
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            if (_processor != null) Flags.multiple_save = false;
            button3.Enabled = true;
        }

        /// <summary>
        /// Name: textBox1_TextChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates the filename flag based on text input.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Flags.File = textBox1.Text;
        }

        private void label8_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Name: textBox2_TextChanged
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Updates the storage folder flag.
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Flags.Folder = textBox2.Text;
        }

        /// <summary>
        /// Name: textBox2_MouseClick
        /// Input: object sender, MouseEventArgs e
        /// Output: None
        /// Description: Opens a folder browser dialog to select the save path.
        /// </summary>
        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
                Flags.Folder = textBox2.Text;
            }
        }

        /// <summary>
        /// Name: HandleButtonOn
        /// Input: bool v
        /// Output: None
        /// Description: Event handler to re-enable button UI when recording finishes.
        /// </summary>
        private void HandleButtonOn(bool v)
        {
            button3.Enabled = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Flags.MaxFilesToSave = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Flags.Delay = (int)numericUpDown2.Value;
        }

        /// <summary>
        /// Name: button5_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Resets background, processor, and clears buffers.
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            Flags.background_reset = true;
            _processor?.ResetBackground();
            _processor?.Reset();
            _processor.ClearBuffers();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _processor?.SaveBackground();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _processor?.BackgroundRead();
        }

        private void ZoomPanel_Load(object sender, EventArgs e) { }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            Flags.SignalOffset = -(int)numericUpDown3.Value;
        }

        /// <summary>
        /// Name: button8_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Opens a color dialog to change the UI text color.
        /// </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDlg = new ColorDialog())
            {
                colorDlg.Color = Flags.CustomTextColor;
                colorDlg.FullOpen = true;

                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    Flags.CustomTextColor = colorDlg.Color;
                    ((Button)sender).BackColor = colorDlg.Color;
                }
            }
        }

        /// <summary>
        /// Name: button9_Click
        /// Input: object sender, EventArgs e
        /// Output: None
        /// Description: Opens a color dialog to change the graph visualization color.
        /// </summary>
        private void button9_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDlg = new ColorDialog())
            {
                colorDlg.Color = Flags.CustomGraphColor;
                colorDlg.FullOpen = true;

                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    Flags.CustomGraphColor = colorDlg.Color;
                    ((Button)sender).BackColor = colorDlg.Color;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Flags.boolUseMedianFilter = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Flags.UseSavitzkyGolay = checkBox2.Checked;
        }

        private void trackBarLevel_Scroll(object sender, EventArgs e) { }

        /// <summary>
        /// Name: trackBarAverage_MouseUp
        /// Input: object sender, MouseEventArgs e
        /// Output: None
        /// Description: Resets the accumulator when the user finishes adjusting the average slider.
        /// </summary>
        private void trackBarAverage_MouseUp(object sender, MouseEventArgs e)
        {
            _processor?.ResetAccumulator();
            _processor?.ClearBuffers();
        }

        private void enablePassiveRadarWindow_CheckedChanged_2(object sender, EventArgs e) { }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Flags.beep_active = checkBoxBeep.Checked;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Flags.credits = true;
        }

        private void checkBoxStat_CheckedChanged(object sender, EventArgs e)
        {
            Flags.statistics = checkBoxStat.Checked;
        }
    }
}

