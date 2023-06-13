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
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLevel)).BeginInit();
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
            this.textBoxGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxGain.Location = new System.Drawing.Point(146, 121);
            this.textBoxGain.Name = "textBoxGain";
            this.textBoxGain.Size = new System.Drawing.Size(65, 15);
            this.textBoxGain.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 97);
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
            this.label4.Location = new System.Drawing.Point(140, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "KamSoft";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Dynamic averaging ";
            // 
            // trackBarAverage
            // 
            this.trackBarAverage.LargeChange = 100;
            this.trackBarAverage.Location = new System.Drawing.Point(3, 231);
            this.trackBarAverage.Maximum = 500;
            this.trackBarAverage.Minimum = 1;
            this.trackBarAverage.Name = "trackBarAverage";
            this.trackBarAverage.Size = new System.Drawing.Size(138, 56);
            this.trackBarAverage.TabIndex = 15;
            this.trackBarAverage.TickFrequency = 50;
            this.trackBarAverage.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarAverage.Value = 10;
            this.trackBarAverage.Scroll += new System.EventHandler(this.trackBarAverage_Scroll);
            this.trackBarAverage.ValueChanged += new System.EventHandler(this.trackBarAverage_ValueChanged);
            // 
            // textBoxAverage
            // 
            this.textBoxAverage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAverage.Location = new System.Drawing.Point(147, 243);
            this.textBoxAverage.Name = "textBoxAverage";
            this.textBoxAverage.Size = new System.Drawing.Size(65, 15);
            this.textBoxAverage.TabIndex = 16;
            this.textBoxAverage.TextChanged += new System.EventHandler(this.textBoxAverage_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 24);
            this.button1.TabIndex = 17;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(108, 342);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 24);
            this.button2.TabIndex = 18;
            this.button2.Text = "Export data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(155, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "v. 2.6";
            this.label2.Click += new System.EventHandler(this.label2_Click);
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
            this.comboBox1.Location = new System.Drawing.Point(138, 46);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(73, 24);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 50);
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
            this.comboBox2.Location = new System.Drawing.Point(138, 70);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(73, 24);
            this.comboBox2.TabIndex = 22;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 72);
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
            this.trackBarLevel.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarLevel.Value = 1;
            this.trackBarLevel.ValueChanged += new System.EventHandler(this.trackBarLevel_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(54, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Level";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // textBoxLevel
            // 
            this.textBoxLevel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLevel.Location = new System.Drawing.Point(147, 171);
            this.textBoxLevel.Name = "textBoxLevel";
            this.textBoxLevel.Size = new System.Drawing.Size(65, 15);
            this.textBoxLevel.TabIndex = 26;
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(3, 282);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(101, 24);
            this.button_stop.TabIndex = 27;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_background
            // 
            this.button_background.Location = new System.Drawing.Point(108, 282);
            this.button_background.Name = "button_background";
            this.button_background.Size = new System.Drawing.Size(101, 24);
            this.button_background.TabIndex = 28;
            this.button_background.Text = "Background";
            this.button_background.UseVisualStyleBackColor = true;
            this.button_background.Click += new System.EventHandler(this.button_background_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(108, 312);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 24);
            this.button3.TabIndex = 29;
            this.button3.Text = "Data clean";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // ZoomPanel
            // 
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button_background);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.textBoxLevel);
            this.Controls.Add(this.label7);
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarGain);
            this.Controls.Add(this.textBoxGain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enablePassiveRadarWindow);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "ZoomPanel";
            this.Size = new System.Drawing.Size(212, 383);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLevel)).EndInit();
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
    }
}
