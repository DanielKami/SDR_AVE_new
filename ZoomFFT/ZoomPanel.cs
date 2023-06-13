﻿using System;
using System.Windows.Forms;
using System.Threading;



namespace SDRSharp.Average
{
    public partial class ZoomPanel : UserControl
    {
        private IFProcessor _ifProcessor;
        private bool State=false;
        private const int multiplayer = 100;
        public ZoomPanel(IFProcessor ifProcessor)
        {
            InitializeComponent();
            _ifProcessor = ifProcessor;

            
           
            trackBarGain.Value = (int)_ifProcessor.Gain;
            textBoxGain.Text = "" + (int)_ifProcessor.Gain;
            trackBarLevel.Value = (int)_ifProcessor.Level;
            textBoxLevel.Text = "" + (int)_ifProcessor.Level;

            int medium = _ifProcessor.average / multiplayer;
            if (medium < 1) medium = 1;
            trackBarAverage.Value = medium ;
            
            textBoxAverage.Text = "" + trackBarAverage.Value * multiplayer * _ifProcessor.Intermediate_average;

            if (_ifProcessor.Max_BufferSize == 16) comboBox1.Text= "16";
            if (_ifProcessor.Max_BufferSize == 32) comboBox1.Text = "32";
            if (_ifProcessor.Max_BufferSize == 64) comboBox1.Text = "64";
            if (_ifProcessor.Max_BufferSize == 128) comboBox1.Text = "128";
            if (_ifProcessor.Max_BufferSize == 256) comboBox1.Text = "256";
            if (_ifProcessor.Max_BufferSize == 512) comboBox1.Text = "512";
            if (_ifProcessor.Max_BufferSize == 1024) comboBox1.Text = "1024";
            if (_ifProcessor.Max_BufferSize == 2048) comboBox1.Text = "2048";
            if (_ifProcessor.Max_BufferSize == 4096) comboBox1.Text = "4096";
            if (_ifProcessor.Max_BufferSize == 8192) comboBox1.Text = "8192";

            if (_ifProcessor.Intermediate_average == 1) comboBox2.Text = "1";
            if (_ifProcessor.Intermediate_average == 10) comboBox2.Text = "10";
            if (_ifProcessor.Intermediate_average == 100) comboBox2.Text = "100";
            if (_ifProcessor.Intermediate_average == 1000) comboBox2.Text = "1000";
            if (_ifProcessor.Intermediate_average == 10000) comboBox2.Text = "10000";


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
            _ifProcessor.Gain = trackBarGain.Value;
            textBoxGain.Text = ""+trackBarGain.Value;
        }


 
        private void trackBarAverage_ValueChanged(object sender, EventArgs e)
        {
            _ifProcessor.average = trackBarAverage.Value  * multiplayer;
            textBoxAverage.Text = "" + trackBarAverage.Value  * multiplayer * _ifProcessor.Intermediate_average;
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

            if (comboBox2.Text == "1") _ifProcessor.Intermediate_average=1;
            if (comboBox2.Text == "10") _ifProcessor.Intermediate_average = 10;
            if (comboBox2.Text == "100") _ifProcessor.Intermediate_average = 100;
            if (comboBox2.Text == "1000") _ifProcessor.Intermediate_average = 1000;
            if (comboBox2.Text == "10000") _ifProcessor.Intermediate_average = 10000;

            textBoxAverage.Text = "" + trackBarAverage.Value * multiplayer * _ifProcessor.Intermediate_average;

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
            _ifProcessor.Level = trackBarLevel.Value;
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
            _ifProcessor.background_recording = true;
        }

        private void enablePassiveRadarWindow_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            _ifProcessor.DataClean();
        }

        private void trackBarAverage_Scroll(object sender, EventArgs e)
        {

        }

        private void textBoxAverage_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
