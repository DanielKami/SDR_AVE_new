/******************************************************************************
 * * PROGRAM NAME:  SDR AVE
 * CLASS:          IFAverageWindow
 * VERSION:        3.x.x
 * * DESCRIPTION:   Advanced Signal Averaging Plugin for #SDR (SDRSharp)
 * Enhances visual representation of the IF spectrum.
 * * AUTHOR:        Daniel M. Kamiński
 * LOCATION:      Lublin 2026, Poland
 * * TARGET REFS:   .NET 9.0 | .NET 8.0
 * COMPATIBILITY: Designed for #SDR by Youssef Touil
 * * ------------------------------------------------------------------------
 * Copyright (c) 2026 Daniel M. Kamiński. All rights reserved.
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SDRSharp.Average
{
    public unsafe partial class IFAverageWindow : Form
    {
        public double* Data;
        public int BufferFrameSize;
        public int CurrentSampleCount;
        public int TargetSampleCount;
        public long CenterFrequency;
        public int SampleRate;

        private int LeftMargin = 65;
        private int RightMargin = 20;
        private int BottomMargin = 45;
        private int TopMargin = 25;

        private Point _mouseLocation;
        private bool _mouseInClient;
        public double MeasuredFrameTime; // Duration of one intermediate frame in seconds

        private DateTime _creditsStartTime = DateTime.MinValue;
        private bool _lastCreditsFlag = false;


        public DateTime LastSaveTimeForDisplay = DateTime.MinValue;
        /// <summary>
        /// NAME: IFAverageWindow (Constructor)
        /// INPUT: None
        /// OUTPUT: Instance of IFAverageWindow
        /// DESCRIPTION: Initializes the window properties, styles, and event handlers for the spectrum display.
        /// </summary>
        public IFAverageWindow()
        {
            this.Text = "Spectrum Collector";
            this.Size = new Size(Flags.window_Width, Flags.window_Height);
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.Paint += IFAverageWindow_Paint;
            this.Resize += (s, e) => this.Invalidate();

            this.MouseMove += (s, e) => { _mouseLocation = e.Location; _mouseInClient = true; this.Invalidate(); };
            this.MouseLeave += (s, e) => { _mouseInClient = false; this.Invalidate(); };
        }

        /// <summary>
        /// NAME: RefreshGraph
        /// INPUT: None
        /// OUTPUT: Void
        /// DESCRIPTION: Forces the window to redraw itself, handling cross-thread calls if necessary.
        /// </summary>
        public void RefreshGraph() { if (!this.IsDisposed) try { if (this.InvokeRequired) this.BeginInvoke(new Action(() => this.Invalidate())); else this.Invalidate(); } catch { } }

        /// <summary>
        /// NAME: GetY
        /// INPUT: double dbValue, float graphHeight, float dbTop, float dbBottom
        /// OUTPUT: float (Y coordinate)
        /// DESCRIPTION: Maps a decibel value to a vertical pixel coordinate on the graph.
        /// </summary>
        private float GetY(double dbValue, float graphHeight, float dbTop, float dbBottom)
        {
            float range = dbTop - dbBottom;
            if (range <= 0.1f) return TopMargin + graphHeight / 2f;   // środek ekranu przy błędzie

            float pct = (float)((dbValue - dbBottom) / range);
            pct = Math.Clamp(pct, -0.5f, 1.5f);   // nie pozwalamy na skrajne NaN-y

            return (TopMargin + graphHeight) - (pct * graphHeight);
        }

        /// <summary>
        /// NAME: IFAverageWindow_Paint
        /// INPUT: object sender, PaintEventArgs e
        /// OUTPUT: Void
        /// DESCRIPTION: Main rendering loop. Draws the grid, scales, signal spectrum, and info panels.
        /// </summary>
        private void IFAverageWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            float w = this.ClientSize.Width;
            float h = this.ClientSize.Height;

            // ────────────────────────────────────────────────
            // Bardzo wczesne wyjście przy absurdalnie małym oknie
            // ────────────────────────────────────────────────
            if (h < 100 || w < 200)   // minimalna sensowna wysokość/szerokość
            {
                using (Font f = new Font("Consolas", 9))
                using (Brush b = new SolidBrush(Color.Silver))
                {
                    g.DrawString("Too small window - expand it vertically!", f, b, 10, 10);
                }
                return;
            }

            float graphWidth = w - LeftMargin - RightMargin;
            float graphHeight = h - TopMargin - BottomMargin;

            // Drugie zabezpieczenie – jeśli po odjęciu marginesów nadal ≤ 0
            if (graphHeight <= 10f)
            {
                using (Font f = new Font("Consolas", 9))
                using (Brush b = new SolidBrush(Color.OrangeRed))
                {
                    g.DrawString("Za mała wysokość okna – powiększ", f, b, 10, h / 2 - 20);
                }
                return;
            }

            float dbSpan = 120.0f / (Flags.Gain > 0 ? Flags.Gain : 1.0f);
            float dbCenter = -60.0f + Flags.Level;
            float dbTop = dbCenter + (dbSpan / 2.0f);
            float dbBottom = dbCenter - (dbSpan / 2.0f);

            // 1. GRID AND SCALES
            DrawGridAndScales(g, w, h, graphWidth, graphHeight, dbTop, dbBottom);

            // 2. Jeśli brak danych – tylko info i koniec
            if (Data == null || BufferFrameSize <= 2 || CurrentSampleCount == 0)
            {
                DrawInfoPanel(g);
                return;
            }

            // 3. Wykres (linia + wypełnienie)
            float xStep = graphWidth / (BufferFrameSize - 1);
            PointF[] points = new PointF[BufferFrameSize];

            for (int i = 0; i < BufferFrameSize; i++)
            {
                double yDb = Data[i] + Flags.SignalOffset;
                float yPos = GetY(yDb, graphHeight, dbTop, dbBottom);

                // Zabezpieczenie przed NaN / ∞
                if (float.IsNaN(yPos) || float.IsInfinity(yPos))
                    yPos = h / 2f;   // środek ekranu w razie błędu

                yPos = Math.Clamp(yPos, TopMargin - 50f, h - BottomMargin + 50f);
                points[i] = new PointF(LeftMargin + (i * xStep), yPos);
            }

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(points);
                path.AddLine(points[BufferFrameSize - 1].X, h - BottomMargin, points[0].X, h - BottomMargin);
                using (LinearGradientBrush br = new LinearGradientBrush(
                    new PointF(0, TopMargin), new PointF(0, h - BottomMargin),
                    Color.FromArgb(120, Flags.CustomGraphColor), Color.Transparent))
                {
                    g.FillPath(br, path);
                }
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.White, 1.0f))
                g.DrawLines(p, points);

            // 4. Mouse tracking i info panel
            DrawMouseTracking(g, w, h, graphWidth, graphHeight, dbTop, dbBottom, xStep, points);
            DrawInfoPanel(g);
        }

        /// <summary>
        /// NAME: DrawGridAndScales
        /// INPUT: Graphics g, float w, float h, float graphWidth, float graphHeight, float dbTop, float dbBottom
        /// OUTPUT: Void
        /// DESCRIPTION: Renders the horizontal dB lines and vertical Frequency (MHz) lines.
        /// </summary>
        private void DrawGridAndScales(Graphics g, float w, float h, float graphWidth, float graphHeight, float dbTop, float dbBottom)
        {
            using (Pen majorGridPen = new Pen(Color.FromArgb(70, 70, 70), 1f))
            using (Pen minorGridPen = new Pen(Color.FromArgb(35, 35, 35), 1f))
            using (Font scaleFont = new Font("Consolas", 8))
            using (Brush scaleBrush = new SolidBrush(Color.Silver))
            {
                // Zabezpieczenie przed zerem / ujemną wysokością / zerowym zakresem dB
                float dbRange = Math.Max(dbTop - dbBottom, 0.1f);
                float graphHeightSafe = Math.Max(graphHeight, 1f);
                float pixelsPer10dB = (graphHeightSafe / dbRange) * 10f;

                // Wybór kroku minor lines
                float minorStep_dB = 10f;
                if (pixelsPer10dB >= 140f) minorStep_dB = 1f;
                else if (pixelsPer10dB >= 80f) minorStep_dB = 2f;
                else if (pixelsPer10dB >= 40f) minorStep_dB = 5f;

                // Start od najbliższej wielokrotności 10 od góry
                double startDb = Math.Ceiling(dbTop / 10.0) * 10.0;

                for (double db = startDb; db >= dbBottom - 1; db -= minorStep_dB)
                {
                    float y = GetY(db, graphHeightSafe, dbTop, dbBottom);

                    // Pomijamy linie poza obszarem widocznym
                    if (float.IsNaN(y) || y < TopMargin - 10 || y > h - BottomMargin + 10)
                        continue;

                    bool isMajor = Math.Abs(db % 10.0) < 0.001;

                    Pen pen = isMajor ? majorGridPen : minorGridPen;
                    g.DrawLine(pen, LeftMargin, y, w - RightMargin, y);

                    if (isMajor)
                    {
                        string label = $"{(int)Math.Round(db)} dB";
                        g.DrawString(label, scaleFont, scaleBrush, 5, y - 7);
                    }
                }

                // Pionowe linie MHz (bez zmian)
                int currentSampleRate = SampleRate > 0 ? SampleRate : 2048000;
                double startFreq = (double)CenterFrequency - (currentSampleRate / 2.0);

                for (int i = 0; i <= 10; i++)
                {
                    float xPos = LeftMargin + (i * (graphWidth / 10f));
                    if (i != 5) g.DrawLine(majorGridPen, xPos, TopMargin, xPos, h - BottomMargin);

                    double freq = startFreq + (i * (currentSampleRate / 10.0));
                    string freqStr = (freq / 1_000_000.0).ToString("F3");
                    g.DrawString(freqStr, scaleFont, scaleBrush, xPos - 20, h - BottomMargin + 5);
                }

                // Linia centralna DC
                using (Pen centerPen = new Pen(Color.FromArgb(150, 120, 0, 0), 2))
                    g.DrawLine(centerPen, LeftMargin + (graphWidth / 2f), TopMargin, LeftMargin + (graphWidth / 2f), h - BottomMargin);
            }
        }



        /// <summary>
        /// NAME: DrawMouseTracking
        /// INPUT: Graphics g, float w, float h, float graphWidth, float graphHeight, float dbTop, float dbBottom, float xStep, PointF[] points
        /// OUTPUT: Void
        /// DESCRIPTION: Draws crosshair and text info for the frequency/power under the mouse cursor.
        /// </summary>
        private void DrawMouseTracking(Graphics g, float w, float h, float graphWidth, float graphHeight, float dbTop, float dbBottom, float xStep, PointF[] points)
        {
            if (_mouseInClient && _mouseLocation.X >= LeftMargin && _mouseLocation.X <= w - RightMargin)
            {
                int dataIndex = Math.Clamp((int)Math.Round((_mouseLocation.X - LeftMargin) / xStep), 0, BufferFrameSize - 1);
                PointF signalPoint = points[dataIndex];

                using (Pen cursorPen = new Pen(Color.Cyan, 1))
                {
                    cursorPen.DashStyle = DashStyle.Dash;
                    g.DrawLine(cursorPen, signalPoint.X, TopMargin, signalPoint.X, h - BottomMargin);
                    g.DrawEllipse(cursorPen, signalPoint.X - 4, signalPoint.Y - 4, 8, 8);
                }

                int currentSampleRate = SampleRate > 0 ? SampleRate : 2048000;
                double mouseFreq = ((double)CenterFrequency - (currentSampleRate / 2.0)) + ((double)dataIndex / (BufferFrameSize - 1) * currentSampleRate);
                double mouseDb = Data[dataIndex] + Flags.SignalOffset;

                string mouseText = $"{(mouseFreq / 1_000_000.0):F6} MHz | {mouseDb:F2} dB";
                using (Font mouseFont = new Font("Consolas", 8, FontStyle.Regular))
                using (Brush textBrush = new SolidBrush(Flags.CustomTextColor))
                {
                    g.DrawString(mouseText, mouseFont, textBrush, 5, h - 20);
                }
            }
        }

        /// <summary>
        /// NAME: DrawInfoPanel
        /// INPUT: Graphics g
        /// OUTPUT: Void
        /// DESCRIPTION: Renders status messages, progress bars, ETA indicators 
        ///              and countdown for multiple save mode.
        /// </summary>
        private void DrawInfoPanel(Graphics g)
        {
            using (Font infoFont = new Font("Consolas", 9, FontStyle.Bold))
            using (Pen borderPen = new Pen(Color.FromArgb(160, Flags.CustomTextColor), 1))
            using (Brush panelBackBrush = new SolidBrush(Color.FromArgb(180, 15, 15, 15)))
            {
                int line = 0;
                var msgs = new List<(string T, Color C)>();

                // --- Progress akwizycji ---
                int currentFrames = CurrentSampleCount * Flags.Intermediate_average;
                int totalTargetFrames = TargetSampleCount * Flags.Intermediate_average;

                string progressText = $"FFT: {BufferFrameSize} | AVG [ {currentFrames} / {totalTargetFrames} ]";

                // ETA do końca bieżącej akwizycji
                if (MeasuredFrameTime > 0 && CurrentSampleCount < TargetSampleCount)
                {
                    int stepsLeft = TargetSampleCount - CurrentSampleCount;
                    double totalSecondsLeft = (stepsLeft * MeasuredFrameTime) / 1000.0;

                    TimeSpan eta = TimeSpan.FromSeconds(totalSecondsLeft);

                    // Formatiowanie: jeśli powyżej godziny, dodaj godziny
                    string etaStr = eta.TotalHours >= 1
                        ? $"{(int)eta.TotalHours:D2}:{eta.Minutes:D2}:{eta.Seconds:D2}"
                        : $"{eta.Minutes:D2}:{eta.Seconds:D2}";

                    progressText += $" | ETA: {etaStr}";
                }

                msgs.Add((progressText, Flags.CustomTextColor));


                // === NOWOŚĆ: Odliczanie wstecz dla Multiple Save ===
                if (Flags.multiple_save)
                {
                    double secondsSinceLastSave = (DateTime.Now - LastSaveTimeForDisplay).TotalSeconds;
                    double delay = Flags.Delay > 0 ? Flags.Delay : 0;

                    int secondsToNext = Math.Max(0, (int)Math.Ceiling(delay - secondsSinceLastSave));

                    string multiText = $"MULTI SAVE → File {Flags.file_count_number + 1}/{Flags.MaxFilesToSave} | Next save in: {secondsToNext}s";

                    msgs.Add((multiText, Color.Orange));
                }

                // --- Statusy background i single save ---
                if (Flags.background_recording)
                {
                    msgs.Add(("STATUS: ACQUIRING BACKGROUND...", Color.Red));
                }
                else if (Flags.background_recorded)
                {
                    msgs.Add(($"STATUS: BACKGROUND ACTIVE ({Flags.BackgroundFileName})", Color.LimeGreen));
                }

                if (Flags.single_save_active)
                {
                    msgs.Add(($"FILE SAVED: {Flags.LastSavedFileName}", Color.Yellow));
                }

                if (Flags.boolUseMedianFilter)
                {
                    msgs.Add(($"RFI FILTER: ACTIVE | REJECTED: {Flags.RejectedFramesCount}", Color.Cyan));
                }

                if (Flags.statistics)
                {
                    msgs.Add(($"BUF: {Flags.LastBlockSizeReceived} → {Flags.LastBlockFullFFTs} FFTs | " +
                              $"Discard: {Flags.LastBlockDiscardedSamples} | " +
                              $"INPUT: {Flags.ProcessCallsPerSecond:F1} calls/s  {Flags.SamplesPerSecond / 1_000_000:F2} MS/s | FFTs/s {Flags.LastBlockFullFFTs * Flags.ProcessCallsPerSecond:F0}" 
                              , Color.Silver));
                  
                }
                // Rysowanie wszystkich wiadomości
                foreach (var m in msgs)
                {
                    SizeF sz = g.MeasureString(m.T, infoFont);
                    RectangleF r = new RectangleF(LeftMargin + 10, TopMargin + 10 + (line * 25), sz.Width + 12, sz.Height + 6);

                    DrawRoundedRectangle(g, panelBackBrush, borderPen, r, 6);

                    using (Brush b = new SolidBrush(m.C))
                    {
                        g.DrawString(m.T, infoFont, b, r.X + 6, r.Y + 3);
                    }

                    line++;
                }


                // 1. Logika aktywacji czasowej
                if (Flags.credits && !_lastCreditsFlag)
                {
                    _creditsStartTime = DateTime.Now;
                }
                _lastCreditsFlag = Flags.credits;

                if (Flags.credits)
                {
                    double elapsed = (DateTime.Now - _creditsStartTime).TotalSeconds;

                    if (elapsed < 5.0)
                    {
                        // Obliczanie przezroczystości (Fade-out w ostatniej sekundzie)
                        int alpha = (elapsed > 4.0) ? (int)((5.0 - elapsed) * 255) : 255;

                        // --- PRZYGOTOWANIE TREŚCI ---
                        string vLine = $"IF Average v{Flags.version}"; // wersja 4.7
                        string devLine = "Developer: dr Daniel M. Kamiński (PL)";
                        string mailLine = "Contact: daniel_kaminski3@wp.pl";
                        string testerLine = "Main Tester & Design Consultant: Alex Petit jr(US)";
                        string locLine = "Lublin 2026, Poland";
                        string freeLine = "This software is Free of Charge for non-commercial use.";
                        string thanksLine = "Special thanks to the Amateur Radio & Radio Astronomy community!";

                        using (Font fTitle = new Font("Segoe UI", 15, FontStyle.Bold))
                        using (Font fMain = new Font("Segoe UI", 11, FontStyle.Regular))
                        using (Font fSmall = new Font("Segoe UI", 9, FontStyle.Italic))
                        {
                            // --- OBLICZANIE WYMIARÓW I CENTROWANIE ---
                            // Mierzymy najdłuższą linię, aby dopasować szerokość tła
                            SizeF maxSz = g.MeasureString(thanksLine, fSmall);
                            float rectW = maxSz.Width + 60;
                            float rectH = 220;
                            float rectX = (this.Width - rectW) / 2;
                            float rectY = (this.Height - rectH) / 2;

                            // --- RYSOWANIE TŁA ---
                            // Ciemny grafit z 90% nieprzezroczystością
                            using (Brush backBrush = new SolidBrush(Color.FromArgb((int)(alpha * 0.9), 15, 15, 15)))
                            using (Pen goldPen = new Pen(Color.FromArgb(alpha, Color.Gold), 2))
                            {
                                RectangleF bgRect = new RectangleF(rectX, rectY, rectW, rectH);
                                g.FillRectangle(backBrush, bgRect);
                                g.DrawRectangle(goldPen, rectX, rectY, rectW, rectH);

                                // --- RYSOWANIE TEKSTÓW ---
                                using (Brush bGold = new SolidBrush(Color.FromArgb(alpha, Color.Gold)))
                                using (Brush bWhite = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                                using (Brush bGray = new SolidBrush(Color.FromArgb(alpha, Color.LightGray)))
                                {
                                    StringFormat fmt = new StringFormat { Alignment = StringAlignment.Center };
                                    float cx = rectX + (rectW / 2);

                                    // Tytuł i wersja
                                    g.DrawString(vLine, fTitle, bGold, cx, rectY + 15, fmt);

                                    // Twórcy i kontakt
                                    g.DrawString(devLine, fMain, bWhite, cx, rectY + 55, fmt);
                                    g.DrawString(mailLine, fSmall, bGray, cx, rectY + 75, fmt);
                                    g.DrawString(testerLine, fMain, bWhite, cx, rectY + 100, fmt);

                                    // Lokalizacja
                                    g.DrawString(locLine, fSmall, bGray, cx, rectY + 125, fmt);

                                    // Separator
                                    g.DrawLine(new Pen(Color.FromArgb(alpha / 2, Color.Gray)), rectX + 40, rectY + 150, rectX + rectW - 40, rectY + 150);

                                    // Stopka (Licencja i Podziękowania)
                                    g.DrawString(freeLine, fSmall, bGray, cx, rectY + 160, fmt);
                                    g.DrawString(thanksLine, fSmall, bGold, cx, rectY + 185, fmt);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Koniec czasu - ukrywamy
                        Flags.credits = false;
                    }
                }
            }
        }

        /// <summary>
        /// NAME: DrawRoundedRectangle
        /// INPUT: Graphics g, Brush brush, Pen pen, RectangleF rect, float radius
        /// OUTPUT: Void
        /// DESCRIPTION: Helper method to draw a filled and outlined rectangle with rounded corners.
        /// </summary>
        private void DrawRoundedRectangle(Graphics g, Brush brush, Pen pen, RectangleF rect, float radius)
        {
            float d = radius * 2;
            if (d <= 0 || rect.Width <= d || rect.Height <= d) return;
            using (GraphicsPath p = new GraphicsPath())
            {
                p.AddArc(rect.X, rect.Y, d, d, 180, 90);
                p.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                p.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                p.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                g.FillPath(brush, p); g.DrawPath(pen, p);
            }
        }
    }
}