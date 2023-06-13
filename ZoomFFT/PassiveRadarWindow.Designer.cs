using System;
using System.Collections.Generic;
using System.Text;



namespace SDRSharp.Average
{
    partial class IFAverageWindow
    {
        #region graphisc definitions
    
        
        #endregion

      //  DispatcherTimer CalculateTimer;
       // GameTimer timer;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelViewport = new System.Windows.Forms.Panel();
            this.SuspendLayout();
             
            // 
            // panelViewport
            // 
            this.panelViewport.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelViewport.Location = new System.Drawing.Point(1, 1);
            this.panelViewport.Name = "panelViewport";
            this.panelViewport.Size = new System.Drawing.Size(436, 385);
            this.panelViewport.TabIndex = 0;
            this.panelViewport.Paint += new System.Windows.Forms.PaintEventHandler(this.panelViewport_Paint);
            this.panelViewport.MouseLeave += new System.EventHandler(this.panelViewport_MouseLeave);
            this.panelViewport.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelViewport_MouseMove);
            this.panelViewport.Move += new System.EventHandler(this.panelViewport_Move);
            this.panelViewport.Resize += new System.EventHandler(this.panelViewport_Resize);
            // 
            // IFAverageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 386);
            this.ControlBox = false;
            this.Controls.Add(this.panelViewport);
            this.Name = "IFAverageWindow";
            this.Text = "IF Average Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PassiveRadarWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PassiveRadarWindow_FormClosed);
            this.ResizeBegin += new System.EventHandler(this.IFAverageWindow_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.IFAverageWindow_ResizeEnd);
            this.ClientSizeChanged += new System.EventHandler(this.IFAverageWindow_ClientSizeChanged);
            this.LocationChanged += new System.EventHandler(this.IFAverageWindow_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.IFAverageWindow_SizeChanged);
            this.ResumeLayout(false);

        }

 #endregion

        private System.Windows.Forms.Panel panelViewport;









    }
}