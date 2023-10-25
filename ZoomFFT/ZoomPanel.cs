using System;
using System.Windows.Forms;
using System.Threading;



namespace SDRSharp.Average
{
    public partial class ZoomPanel : UserControl
    {
        private IFProcessor _ifProcessor;
        private bool State=false;
        public ZoomPanel(IFProcessor ifProcessor)
        {
            InitializeComponent();
            _ifProcessor = ifProcessor;



            IFAverageWindow.SomethingHappened += new IFAverageWindow.MyEventHandler(HandleWindowIF_Close);
            IFProcessor.Recording += new IFProcessor.MyEventHandler(HandleButtonOn);

            trackBarGain.Value = (int)Flags.Gain;
            textBoxGain.Text = "" + (int)Flags.Gain;
            trackBarLevel.Value = (int)Flags.Level;
            textBoxLevel.Text = "" + (int)Flags.Level;

            trackBarAverage.Value = Flags.Average;
            textBoxAverage.Text = "" + trackBarAverage.Value * Flags.Intermediate_average;

            if (Flags.Max_BufferSize == 16) comboBox1.Text= "16";
            if (Flags.Max_BufferSize == 32) comboBox1.Text = "32";
            if (Flags.Max_BufferSize == 64) comboBox1.Text = "64";
            if (Flags.Max_BufferSize == 128) comboBox1.Text = "128";
            if (Flags.Max_BufferSize == 256) comboBox1.Text = "256";
            if (Flags.Max_BufferSize == 512) comboBox1.Text = "512";
            if (Flags.Max_BufferSize == 1024) comboBox1.Text = "1024";
            if (Flags.Max_BufferSize == 2048) comboBox1.Text = "2048";
            if (Flags.Max_BufferSize == 4096) comboBox1.Text = "4096";
            if (Flags.Max_BufferSize == 8192) comboBox1.Text = "8192";

            if (Flags.Intermediate_average == 1) comboBox2.Text = "1";
            if (Flags.Intermediate_average == 10) comboBox2.Text = "10";
            if (Flags.Intermediate_average == 100) comboBox2.Text = "100";
            if (Flags.Intermediate_average == 1000) comboBox2.Text = "1000";
            if (Flags.Intermediate_average == 10000) comboBox2.Text = "10000";

            numericUpDown1.Value = Flags.MaxFilesToSave;
            numericUpDown2.Value = Flags.Delay;
            textBox2.Text = Flags.Folder;
            textBox1.Text = Flags.File;
        }

        private void HandleWindowIF_Close(int v)
        {
            enablePassiveRadarWindow.Checked = false;
        }

        public void OffBox()
        {
            enablePassiveRadarWindow.Checked = false;
        }

        private void enablePassiveRadarWindow_CheckedChanged(object sender, EventArgs e)
        {
            _ifProcessor.RestartIFWindow();
            Thread.Sleep(500);
            
           if(enablePassiveRadarWindow.Checked==true)
               _ifProcessor.ControlPassiveRadarWindow();
           else
               _ifProcessor.ControlPassiveRadarWindowHide();

        }

        private void trackBarGain_ValueChanged(object sender, EventArgs e)
        {
            Flags.Gain = trackBarGain.Value;
            textBoxGain.Text = ""+trackBarGain.Value;
        }

        private void trackBarAverage_ValueChanged(object sender, EventArgs e)
        {
            Flags.Average = trackBarAverage.Value;
            textBoxAverage.Text = "" + trackBarAverage.Value * Flags.Intermediate_average;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ifProcessor.Reset();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ifProcessor.Save();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool state = enablePassiveRadarWindow.Checked;

            if (comboBox1.Text == "16") _ifProcessor.UpdateMainBuffer(16, state);
            if (comboBox1.Text == "32") _ifProcessor.UpdateMainBuffer(32, state);
            if (comboBox1.Text == "64") _ifProcessor.UpdateMainBuffer(64, state);
            if (comboBox1.Text == "128") _ifProcessor.UpdateMainBuffer(128, state);
            if (comboBox1.Text == "256") _ifProcessor.UpdateMainBuffer(256, state);
            if (comboBox1.Text == "512") _ifProcessor.UpdateMainBuffer(512, state);
            if (comboBox1.Text == "1024") _ifProcessor.UpdateMainBuffer(1024, state);
            if (comboBox1.Text == "2048") _ifProcessor.UpdateMainBuffer(2048, state);
            if (comboBox1.Text == "4096") _ifProcessor.UpdateMainBuffer(4096, state);
            if (comboBox1.Text == "8192") _ifProcessor.UpdateMainBuffer(8192, state);         
            

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.Text == "1") Flags.Intermediate_average =1;
            if (comboBox2.Text == "10") Flags.Intermediate_average = 10;
            if (comboBox2.Text == "100") Flags.Intermediate_average = 100;
            if (comboBox2.Text == "1000") Flags.Intermediate_average = 1000;
            if (comboBox2.Text == "10000") Flags.Intermediate_average = 10000;

            textBoxAverage.Text = "" + trackBarAverage.Value * Flags.Intermediate_average;

            _ifProcessor.Reset();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void trackBarGain_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBarLevel_ValueChanged(object sender, EventArgs e)
        {
            Flags.Level = trackBarLevel.Value;
            textBoxLevel.Text = "" + trackBarLevel.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (State == false)
            {
                button_stop.Text = "Start";
                _ifProcessor.StopRecording();
                State = true;
            }
            else
            {
                button_stop.Text = "Stop";
                _ifProcessor.StartRecording();
                State = false;
            }
        }

        private void button_background_Click(object sender, EventArgs e)
        {
            Flags.background_recording = true;
        }

        private void enablePassiveRadarWindow_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
 

            button3.Enabled = false;
            _ifProcessor.SaveMultiple();
        }

        private void trackBarAverage_Scroll(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            _ifProcessor.multiple_save = false;
            button3.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Flags.File = textBox1.Text;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Flags.Folder = textBox2.Text;
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

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

        private void button5_Click(object sender, EventArgs e)
        {
            Flags.background_reset = true;
        }
    }
}
