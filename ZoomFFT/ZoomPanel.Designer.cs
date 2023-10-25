using System;

namespace SDRSharp.Average
{
    partial class ZoomPanel
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
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.enablePassiveRadarWindow = new System.Windows.Forms.CheckBox();
            this.trackBarGain = new System.Windows.Forms.TrackBar();
            this.textBoxGain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBarAverage = new System.Windows.Forms.TrackBar();
            this.textBoxAverage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarLevel = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxLevel = new System.Windows.Forms.TextBox();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_background = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLevel)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.AutoSize = true;
            this.mainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.07843F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.92157F));
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(3, 51);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 4;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(0, 0);
            this.mainTableLayoutPanel.TabIndex = 1;
            // 
            // enablePassiveRadarWindow
            // 
            this.enablePassiveRadarWindow.AutoSize = true;
            this.enablePassiveRadarWindow.Location = new System.Drawing.Point(3, 3);
            this.enablePassiveRadarWindow.Name = "enablePassiveRadarWindow";
            this.enablePassiveRadarWindow.Size = new System.Drawing.Size(77, 20);
            this.enablePassiveRadarWindow.TabIndex = 4;
            this.enablePassiveRadarWindow.Text = "Window";
            this.enablePassiveRadarWindow.UseVisualStyleBackColor = true;
            this.enablePassiveRadarWindow.CheckedChanged += new System.EventHandler(this.enablePassiveRadarWindow_CheckedChanged_1);
            this.enablePassiveRadarWindow.CheckStateChanged += new System.EventHandler(this.enablePassiveRadarWindow_CheckedChanged);
            // 
            // trackBarGain
            // 
            this.trackBarGain.LargeChange = 1;
            this.trackBarGain.Location = new System.Drawing.Point(2, 111);
            this.trackBarGain.Maximum = 400;
            this.trackBarGain.Minimum = 1;
            this.trackBarGain.Name = "trackBarGain";
            this.trackBarGain.Size = new System.Drawing.Size(138, 56);
            this.trackBarGain.TabIndex = 5;
            this.trackBarGain.TickFrequency = 100;
            this.trackBarGain.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarGain.Value = 1;
            this.trackBarGain.Scroll += new System.EventHandler(this.trackBarGain_Scroll);
            this.trackBarGain.ValueChanged += new System.EventHandler(this.trackBarGain_ValueChanged);
            // 
            // textBoxGain
            // 
            this.textBoxGain.Location = new System.Drawing.Point(149, 121);
            this.textBoxGain.Name = "textBoxGain";
            this.textBoxGain.Size = new System.Drawing.Size(63, 22);
            this.textBoxGain.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Gain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(146, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "KamSoft";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Dynamic averaging ";
            // 
            // trackBarAverage
            // 
            this.trackBarAverage.LargeChange = 10;
            this.trackBarAverage.Location = new System.Drawing.Point(3, 211);
            this.trackBarAverage.Maximum = 50000;
            this.trackBarAverage.Minimum = 1;
            this.trackBarAverage.Name = "trackBarAverage";
            this.trackBarAverage.Size = new System.Drawing.Size(138, 56);
            this.trackBarAverage.TabIndex = 15;
            this.trackBarAverage.TickFrequency = 5000;
            this.trackBarAverage.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarAverage.Value = 10;
            this.trackBarAverage.Scroll += new System.EventHandler(this.trackBarAverage_Scroll);
            this.trackBarAverage.ValueChanged += new System.EventHandler(this.trackBarAverage_ValueChanged);
            // 
            // textBoxAverage
            // 
            this.textBoxAverage.Location = new System.Drawing.Point(150, 220);
            this.textBoxAverage.Name = "textBoxAverage";
            this.textBoxAverage.Size = new System.Drawing.Size(62, 22);
            this.textBoxAverage.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(2, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 24);
            this.button1.TabIndex = 17;
            this.button1.Text = "Reset ave.";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(107, 342);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 24);
            this.button2.TabIndex = 18;
            this.button2.Text = "Save single ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.Firebrick;
            this.label2.Location = new System.Drawing.Point(146, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "v. 2.7";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048"});
            this.comboBox1.Location = new System.Drawing.Point(149, 46);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(62, 24);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "FFT resolution";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "10",
            "100",
            "1000",
            "10000"});
            this.comboBox2.Location = new System.Drawing.Point(149, 70);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(62, 24);
            this.comboBox2.TabIndex = 22;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-1, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Intermediate average";
            // 
            // trackBarLevel
            // 
            this.trackBarLevel.LargeChange = 1;
            this.trackBarLevel.Location = new System.Drawing.Point(3, 162);
            this.trackBarLevel.Maximum = 1000;
            this.trackBarLevel.Minimum = 1;
            this.trackBarLevel.Name = "trackBarLevel";
            this.trackBarLevel.Size = new System.Drawing.Size(138, 56);
            this.trackBarLevel.TabIndex = 24;
            this.trackBarLevel.TickFrequency = 100;
            this.trackBarLevel.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarLevel.Value = 1;
            this.trackBarLevel.ValueChanged += new System.EventHandler(this.trackBarLevel_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(54, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Level";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // textBoxLevel
            // 
            this.textBoxLevel.Location = new System.Drawing.Point(149, 171);
            this.textBoxLevel.Name = "textBoxLevel";
            this.textBoxLevel.Size = new System.Drawing.Size(63, 22);
            this.textBoxLevel.TabIndex = 26;
            // 
            // button_stop
            // 
            this.button_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_stop.Location = new System.Drawing.Point(2, 312);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(101, 24);
            this.button_stop.TabIndex = 27;
            this.button_stop.Text = "Stop/start ave.";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_background
            // 
            this.button_background.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_background.Location = new System.Drawing.Point(107, 312);
            this.button_background.Name = "button_background";
            this.button_background.Size = new System.Drawing.Size(101, 24);
            this.button_background.TabIndex = 28;
            this.button_background.Text = "Acq. backgr.";
            this.button_background.UseVisualStyleBackColor = true;
            this.button_background.Click += new System.EventHandler(this.button_background_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(106, 157);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 24);
            this.button3.TabIndex = 29;
            this.button3.Text = "Multiple save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(-1, 158);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 24);
            this.button4.TabIndex = 30;
            this.button4.Text = "Stop";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Location = new System.Drawing.Point(3, 416);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 191);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multiple files";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 126);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 22);
            this.textBox1.TabIndex = 31;
            this.textBox1.Text = "data";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(143, 45);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(62, 22);
            this.numericUpDown2.TabIndex = 39;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(-3, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 16);
            this.label11.TabIndex = 38;
            this.label11.Text = "Delay between rec. [s]";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 86);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(199, 22);
            this.textBox2.TabIndex = 37;
            this.textBox2.Text = "c:\\";
            this.textBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseClick);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 16);
            this.label10.TabIndex = 36;
            this.label10.Text = "Path";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(143, 19);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(62, 22);
            this.numericUpDown1.TabIndex = 35;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(-3, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 16);
            this.label9.TabIndex = 33;
            this.label9.Text = "Number of files to save";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 16);
            this.label8.TabIndex = 32;
            this.label8.Text = "Files name";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button5.Location = new System.Drawing.Point(107, 282);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(101, 24);
            this.button5.TabIndex = 32;
            this.button5.Text = "Reset backgr.";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // ZoomPanel
            // 
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_background);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.textBoxLevel);
            this.Controls.Add(this.trackBarLevel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxAverage);
            this.Controls.Add(this.trackBarAverage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarGain);
            this.Controls.Add(this.textBoxGain);
            this.Controls.Add(this.enablePassiveRadarWindow);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "ZoomPanel";
            this.Size = new System.Drawing.Size(212, 610);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLevel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

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
    }
}
