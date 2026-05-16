using System.Windows.Forms;

namespace SDRSharp.Average
{
    partial class ControlPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainTableLayoutPanel = new TableLayoutPanel();
            enablePassiveRadarWindow = new CheckBox();
            trackBarGain = new TrackBar();
            textBoxGain = new TextBox();
            label1 = new Label();
            label4 = new Label();
            label5 = new Label();
            trackBarAverage = new TrackBar();
            textBoxAverage = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label3 = new Label();
            comboBox2 = new ComboBox();
            label6 = new Label();
            trackBarLevel = new TrackBar();
            label7 = new Label();
            textBoxLevel = new TextBox();
            button_stop = new Button();
            button_background = new Button();
            button3 = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button4 = new Button();
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            numericUpDown2 = new NumericUpDown();
            label11 = new Label();
            textBox2 = new TextBox();
            label10 = new Label();
            numericUpDown1 = new NumericUpDown();
            label9 = new Label();
            label8 = new Label();
            button5 = new Button();
            label12 = new Label();
            button6 = new Button();
            button7 = new Button();
            numericUpDown3 = new NumericUpDown();
            label13 = new Label();
            button8 = new Button();
            button9 = new Button();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            toolTip1 = new ToolTip(components);
            checkBoxBeep = new CheckBox();
            checkBoxStat = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)trackBarGain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarAverage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarLevel).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.AutoSize = true;
            mainTableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainTableLayoutPanel.ColumnCount = 2;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.07843F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.92157F));
            mainTableLayoutPanel.Location = new System.Drawing.Point(3, 51);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 4;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            mainTableLayoutPanel.RowStyles.Add(new RowStyle());
            mainTableLayoutPanel.Size = new System.Drawing.Size(0, 0);
            mainTableLayoutPanel.TabIndex = 1;
            // 
            // enablePassiveRadarWindow
            // 
            enablePassiveRadarWindow.AutoSize = true;
            enablePassiveRadarWindow.Location = new System.Drawing.Point(3, 3);
            enablePassiveRadarWindow.Name = "enablePassiveRadarWindow";
            enablePassiveRadarWindow.Size = new System.Drawing.Size(86, 24);
            enablePassiveRadarWindow.TabIndex = 4;
            enablePassiveRadarWindow.Text = "Window";
            toolTip1.SetToolTip(enablePassiveRadarWindow, "Shows cumulate window.");
            enablePassiveRadarWindow.UseVisualStyleBackColor = true;
            enablePassiveRadarWindow.CheckedChanged += enablePassiveRadarWindow_CheckedChanged_2;
            enablePassiveRadarWindow.CheckStateChanged += enablePassiveRadarWindow_CheckedChanged;
            // 
            // trackBarGain
            // 
            trackBarGain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBarGain.LargeChange = 1;
            trackBarGain.Location = new System.Drawing.Point(2, 119);
            trackBarGain.Maximum = 1500;
            trackBarGain.Minimum = 1;
            trackBarGain.Name = "trackBarGain";
            trackBarGain.Size = new System.Drawing.Size(137, 56);
            trackBarGain.TabIndex = 5;
            trackBarGain.TickFrequency = 100;
            trackBarGain.TickStyle = TickStyle.TopLeft;
            toolTip1.SetToolTip(trackBarGain, "Gail of the signal");
            trackBarGain.Value = 1;
            trackBarGain.Scroll += trackBarGain_Scroll;
            trackBarGain.ValueChanged += trackBarGain_ValueChanged;
            // 
            // textBoxGain
            // 
            textBoxGain.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxGain.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            textBoxGain.Location = new System.Drawing.Point(146, 128);
            textBoxGain.Name = "textBoxGain";
            textBoxGain.Size = new System.Drawing.Size(66, 25);
            textBoxGain.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label1.Location = new System.Drawing.Point(9, 107);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(34, 17);
            label1.TabIndex = 5;
            label1.Text = "Gain";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 238);
            label4.ForeColor = System.Drawing.Color.Red;
            label4.Location = new System.Drawing.Point(146, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(66, 18);
            label4.TabIndex = 13;
            label4.Text = "KamSoft";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label5.Location = new System.Drawing.Point(9, 213);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(123, 17);
            label5.TabIndex = 14;
            label5.Text = "Dynamic averaging ";
            // 
            // trackBarAverage
            // 
            trackBarAverage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBarAverage.LargeChange = 10;
            trackBarAverage.Location = new System.Drawing.Point(3, 227);
            trackBarAverage.Maximum = 10000;
            trackBarAverage.Minimum = 1;
            trackBarAverage.Name = "trackBarAverage";
            trackBarAverage.Size = new System.Drawing.Size(144, 56);
            trackBarAverage.TabIndex = 15;
            trackBarAverage.TickFrequency = 5000;
            trackBarAverage.TickStyle = TickStyle.TopLeft;
            toolTip1.SetToolTip(trackBarAverage, "2 stage of averaging sygnal");
            trackBarAverage.Value = 10;
            trackBarAverage.Scroll += trackBarAverage_Scroll;
            trackBarAverage.ValueChanged += trackBarAverage_ValueChanged;
            trackBarAverage.MouseUp += trackBarAverage_MouseUp;
            // 
            // textBoxAverage
            // 
            textBoxAverage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxAverage.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            textBoxAverage.Location = new System.Drawing.Point(146, 228);
            textBoxAverage.Name = "textBoxAverage";
            textBoxAverage.Size = new System.Drawing.Size(65, 25);
            textBoxAverage.TabIndex = 16;
            // 
            // button1
            // 
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button1.Location = new System.Drawing.Point(2, 275);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(96, 24);
            button1.TabIndex = 17;
            button1.Text = "Reset ave.";
            toolTip1.SetToolTip(button1, "Reset all the tables and starts the aquisition from beginning.");
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button2.Location = new System.Drawing.Point(109, 335);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(99, 24);
            button2.TabIndex = 18;
            button2.Text = "Save single ";
            toolTip1.SetToolTip(button2, "Save single data file.");
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.FlatStyle = FlatStyle.System;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 238);
            label2.ForeColor = System.Drawing.Color.Firebrick;
            label2.Location = new System.Drawing.Point(154, 20);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(21, 15);
            label2.TabIndex = 19;
            label2.Text = "v. ";
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "64", "128", "256", "512", "1024", "2048", "4096", "8192" });
            comboBox1.Location = new System.Drawing.Point(151, 46);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(62, 25);
            comboBox1.TabIndex = 20;
            toolTip1.SetToolTip(comboBox1, "FFT resolution");
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label3.Location = new System.Drawing.Point(3, 57);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(89, 17);
            label3.TabIndex = 21;
            label3.Text = "FFT resolution";
            // 
            // comboBox2
            // 
            comboBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "1", "10", "100", "1000", "10000" });
            comboBox2.Location = new System.Drawing.Point(151, 77);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(62, 25);
            comboBox2.TabIndex = 22;
            toolTip1.SetToolTip(comboBox2, "First step of averaging. The sygnal is calculated from mubber of frames selected in window.");
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label6.Location = new System.Drawing.Point(3, 83);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(132, 17);
            label6.TabIndex = 23;
            label6.Text = "Intermediate average";
            // 
            // trackBarLevel
            // 
            trackBarLevel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBarLevel.LargeChange = 1;
            trackBarLevel.Location = new System.Drawing.Point(3, 170);
            trackBarLevel.Maximum = 100;
            trackBarLevel.Minimum = -300;
            trackBarLevel.Name = "trackBarLevel";
            trackBarLevel.Size = new System.Drawing.Size(144, 56);
            trackBarLevel.TabIndex = 24;
            trackBarLevel.TickFrequency = 300;
            trackBarLevel.TickStyle = TickStyle.TopLeft;
            toolTip1.SetToolTip(trackBarLevel, "Level of the signal");
            trackBarLevel.Value = 1;
            trackBarLevel.Scroll += trackBarLevel_Scroll;
            trackBarLevel.ValueChanged += trackBarLevel_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label7.Location = new System.Drawing.Point(9, 157);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(37, 17);
            label7.TabIndex = 25;
            label7.Text = "Level";
            label7.Click += label7_Click;
            // 
            // textBoxLevel
            // 
            textBoxLevel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxLevel.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            textBoxLevel.Location = new System.Drawing.Point(146, 179);
            textBoxLevel.Name = "textBoxLevel";
            textBoxLevel.Size = new System.Drawing.Size(66, 25);
            textBoxLevel.TabIndex = 26;
            // 
            // button_stop
            // 
            button_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button_stop.Location = new System.Drawing.Point(2, 305);
            button_stop.Name = "button_stop";
            button_stop.Size = new System.Drawing.Size(96, 24);
            button_stop.TabIndex = 27;
            button_stop.Text = "Stop/start ave.";
            toolTip1.SetToolTip(button_stop, "Stop/Start recording the signal.");
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button3_Click;
            // 
            // button_background
            // 
            button_background.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_background.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button_background.Location = new System.Drawing.Point(109, 305);
            button_background.Name = "button_background";
            button_background.Size = new System.Drawing.Size(99, 24);
            button_background.TabIndex = 28;
            button_background.Text = "Acq. backgr.";
            toolTip1.SetToolTip(button_background, "Start background aquisition.");
            button_background.UseVisualStyleBackColor = true;
            button_background.Click += button_background_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button3.Location = new System.Drawing.Point(103, 195);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(100, 24);
            button3.TabIndex = 29;
            button3.Text = "Multiple save";
            toolTip1.SetToolTip(button3, "Start saving multiple files.");
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button4
            // 
            button4.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button4.Location = new System.Drawing.Point(7, 195);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(79, 24);
            button4.TabIndex = 30;
            button4.Text = "Stop";
            toolTip1.SetToolTip(button4, "Stop saving multiple files.");
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(numericUpDown2);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            groupBox1.Location = new System.Drawing.Point(3, 370);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(209, 232);
            groupBox1.TabIndex = 31;
            groupBox1.TabStop = false;
            groupBox1.Text = "Multiple files";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            textBox1.Location = new System.Drawing.Point(9, 161);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(194, 25);
            textBox1.TabIndex = 31;
            textBox1.Text = "data";
            toolTip1.SetToolTip(textBox1, "Name of  multiple files. Files have additional numbers according to the max. number selected in numerator number of files.");
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDown2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            numericUpDown2.Location = new System.Drawing.Point(143, 55);
            numericUpDown2.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new System.Drawing.Size(62, 25);
            numericUpDown2.TabIndex = 39;
            numericUpDown2.TextAlign = HorizontalAlignment.Right;
            toolTip1.SetToolTip(numericUpDown2, "Waiting time between multiple files.");
            numericUpDown2.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label11.Location = new System.Drawing.Point(6, 59);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(126, 15);
            label11.TabIndex = 38;
            label11.Text = "Delay between rec. [s]";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            textBox2.Location = new System.Drawing.Point(9, 102);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(194, 25);
            textBox2.TabIndex = 37;
            textBox2.Text = "c:\\";
            toolTip1.SetToolTip(textBox2, "Path of the destination place for the multiple files.");
            textBox2.MouseClick += textBox2_MouseClick;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label10.Location = new System.Drawing.Point(9, 82);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(33, 17);
            label10.TabIndex = 36;
            label10.Text = "Path";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDown1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            numericUpDown1.Location = new System.Drawing.Point(143, 19);
            numericUpDown1.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(62, 25);
            numericUpDown1.TabIndex = 35;
            numericUpDown1.TextAlign = HorizontalAlignment.Right;
            toolTip1.SetToolTip(numericUpDown1, "Number of multiple files.");
            numericUpDown1.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label9.Location = new System.Drawing.Point(5, 25);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(131, 15);
            label9.TabIndex = 33;
            label9.Text = "Number of files to save";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label8.Location = new System.Drawing.Point(9, 141);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(69, 17);
            label8.TabIndex = 32;
            label8.Text = "Files name";
            label8.Click += label8_Click;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button5.Location = new System.Drawing.Point(109, 275);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(99, 24);
            button5.TabIndex = 32;
            button5.Text = "Reset backgr.";
            toolTip1.SetToolTip(button5, "Reset background table.");
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Left;
            label12.AutoSize = true;
            label12.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label12.Location = new System.Drawing.Point(3, 608);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(98, 17);
            label12.TabIndex = 40;
            label12.Text = "Background file";
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Left;
            button6.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button6.Location = new System.Drawing.Point(4, 633);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(118, 24);
            button6.TabIndex = 40;
            button6.Text = "Backgr.  Save";
            toolTip1.SetToolTip(button6, "Save background file.");
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Left;
            button7.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button7.Location = new System.Drawing.Point(4, 663);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(118, 24);
            button7.TabIndex = 41;
            button7.Text = "Backgr.  Read";
            toolTip1.SetToolTip(button7, "Read background file.");
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Anchor = AnchorStyles.Right;
            numericUpDown3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            numericUpDown3.Location = new System.Drawing.Point(149, 707);
            numericUpDown3.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new System.Drawing.Size(62, 25);
            numericUpDown3.TabIndex = 42;
            numericUpDown3.TextAlign = HorizontalAlignment.Right;
            toolTip1.SetToolTip(numericUpDown3, "Scale correction in [dB].");
            numericUpDown3.Value = new decimal(new int[] { 237, 0, 0, 0 });
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Left;
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            label13.Location = new System.Drawing.Point(3, 710);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(119, 15);
            label13.TabIndex = 40;
            label13.Text = "Scale correction [dB]";
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Right;
            button8.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button8.Location = new System.Drawing.Point(122, 836);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(82, 26);
            button8.TabIndex = 43;
            button8.Text = "Text color";
            toolTip1.SetToolTip(button8, "Changes the color of text.");
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Anchor = AnchorStyles.Left;
            button9.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button9.Location = new System.Drawing.Point(9, 836);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(82, 26);
            button9.TabIndex = 44;
            button9.Text = "Plot color";
            toolTip1.SetToolTip(button9, "Changes the color of the graph.");
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Left;
            checkBox1.AutoSize = true;
            checkBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            checkBox1.Location = new System.Drawing.Point(9, 746);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(104, 21);
            checkBox1.TabIndex = 45;
            checkBox1.Text = "Median filter";
            toolTip1.SetToolTip(checkBox1, "Median filter. This filter removes bad frames if they are not enough simmilar to the average. It is applicable also to the saved data.");
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.Anchor = AnchorStyles.Left;
            checkBox2.AutoSize = true;
            checkBox2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            checkBox2.Location = new System.Drawing.Point(9, 773);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(138, 21);
            checkBox2.TabIndex = 46;
            checkBox2.Text = "SavitzkyGolay filter";
            toolTip1.SetToolTip(checkBox2, "S-G filter is only for visualisation. It makes the plot more smuth.");
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBoxBeep
            // 
            checkBoxBeep.Anchor = AnchorStyles.Left;
            checkBoxBeep.AutoSize = true;
            checkBoxBeep.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            checkBoxBeep.Location = new System.Drawing.Point(8, 800);
            checkBoxBeep.Name = "checkBoxBeep";
            checkBoxBeep.Size = new System.Drawing.Size(59, 21);
            checkBoxBeep.TabIndex = 47;
            checkBoxBeep.Text = "Beep";
            toolTip1.SetToolTip(checkBoxBeep, "S-G filter is only for visualisation. It makes the plot more smuth.");
            checkBoxBeep.UseVisualStyleBackColor = true;
            checkBoxBeep.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // checkBoxStat
            // 
            checkBoxStat.Anchor = AnchorStyles.Left;
            checkBoxStat.AutoSize = true;
            checkBoxStat.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            checkBoxStat.Location = new System.Drawing.Point(145, 800);
            checkBoxStat.Name = "checkBoxStat";
            checkBoxStat.Size = new System.Drawing.Size(52, 21);
            checkBoxStat.TabIndex = 48;
            checkBoxStat.Text = "Stat";
            toolTip1.SetToolTip(checkBoxStat, "S-G filter is only for visualisation. It makes the plot more smuth.");
            checkBoxStat.UseVisualStyleBackColor = true;
            checkBoxStat.CheckedChanged += checkBoxStat_CheckedChanged;
            // 
            // ControlPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(checkBoxStat);
            Controls.Add(checkBoxBeep);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(button5);
            Controls.Add(label1);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(groupBox1);
            Controls.Add(button_background);
            Controls.Add(button_stop);
            Controls.Add(textBoxLevel);
            Controls.Add(label6);
            Controls.Add(comboBox2);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBoxAverage);
            Controls.Add(trackBarAverage);
            Controls.Add(label4);
            Controls.Add(textBoxGain);
            Controls.Add(enablePassiveRadarWindow);
            Controls.Add(mainTableLayoutPanel);
            Controls.Add(trackBarLevel);
            Controls.Add(trackBarGain);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(label12);
            Controls.Add(label13);
            Controls.Add(numericUpDown3);
            Controls.Add(button9);
            Controls.Add(button8);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ControlPanel";
            Size = new System.Drawing.Size(219, 865);
            Load += ZoomPanel_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarGain).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarAverage).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarLevel).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.CheckBox enablePassiveRadarWindow;
        private System.Windows.Forms.TrackBar trackBarGain;
        private System.Windows.Forms.TextBox textBoxGain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarAverage;
        private System.Windows.Forms.TextBox textBoxAverage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackBarLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxLevel;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_background;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;


        #endregion

        private NumericUpDown numericUpDown3;
        private Label label13;
        private Button button8;
        private Button button9;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private ToolTip toolTip1;
        private CheckBox checkBoxBeep;
        private CheckBox checkBoxStat;
    }
}
