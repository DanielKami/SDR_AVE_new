 
/******************************************************************************
 * * PROGRAM NAME:  SDR AVE
 * CLASS:           Flags
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

using SDRSharp.Radio;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace SDRSharp.Average
{
    static class Flags
    {
        public static string version = "4.9";

        // Multiple files save settings
        public static string Folder = "";
        public static string File = "data";
        public static string FileBackground = "background";
        public static int MaxFilesToSave = 100;
        public static long Delay = 1000;
        public static bool multiple_save;
        public static int file_count_number;

        // Window & UI settings
        public static int window_Height = 400, window_Width = 600;
        public static float Gain = 1;
        public static float Level = 1;
        public static float SignalOffset = 0;
        public static int Average = 10;
        public static int Intermediate_average = 10;
        public static int Max_BufferSize = 1024;

        // Colors
        public static Color CustomTextColor = Color.Yellow;
        public static Color CustomGraphColor = Color.Blue;

        // --- RESTORED LOGICAL FLAGS (Processor Triggers) ---
        public static bool start_stop = true;
        public static bool background_reset = false;
        public static bool single_save_active;
        public static bool background_save_active;
        public static bool waiting_for_delay = false;

        //Sound beep
        public static bool beep_active;
        public static bool ext_header_active;

        // Operational parameters
        public static int average_max = 50000;
        public static string LastSavedFileName = "";
        public static string BackgroundFileName = "";
        public static DateTime BackgroundSaveTime = DateTime.MinValue;
        public static DateTime LastDataSaveTime = DateTime.MinValue;
        public static int Intermediate_cumulateIndex = 0;
        public static bool background_recording = false;
        public static bool background_recorded = false;
        public static bool ResultsAreSQRT = true;
        public static bool credits = false;

        public static string settings_path = "SDRSharp.Average.settings.txt";

        // Filters
        public static bool boolUseMedianFilter;
        public static int RejectedFramesCount = 0; // Counter for rejected frames
        public static double RFITreshold = 10.0; // Rejection threshold (e.g., 10 dB above median)

        public static bool UseSavitzkyGolay;

        /*
         * When you record a background (noise floor) for a certain number of averages (e.g. 1000 blocks) and then subtract it from the main signal (also averaged over ~1000 blocks), the difference should theoretically be very close to 0 dB in areas without any real signal.
           However, after the background is captured, in every subsequent averaging cycle the subtracted result drifts — typically by 1.5–2.5 dB (sometimes more). The noise floor no longer sits cleanly at 0 dB, but slowly rises or falls.
         */
        // static double biasCorrection = 0.1;   //Small offset for 

        /* * Name: save
         * Input: None
         * Output: Void
         * Description: Persists current application settings and UI states to a text file.
         */


        // Statystyki przetwarzania buforów
        public static bool statistics = true;
        public static int LastBlockSizeReceived = 0;        // rozmiar bufora od SDR#
        public static int LastBlockFullFFTs = 0;            // ile pełnych ramek FFT wygenerowano
        public static int LastBlockDiscardedSamples = 0;    // ile próbek odrzucono z tego bloku
 
        public static double SamplesPerSecond = 0.0;
        public static double ProcessCallsPerSecond = 0.0;

        public static void save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(settings_path))
                {
                    sw.WriteLine("window_Height " + window_Height);
                    sw.WriteLine("window_Width " + window_Width);
                    sw.WriteLine("Gain " + Gain.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine("Level " + Level.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine("SignalOffset " + SignalOffset.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine("Average " + Average);
                    sw.WriteLine("Intermediate_average " + Intermediate_average);
                    sw.WriteLine("Max_BufferSize " + Max_BufferSize);
                    sw.WriteLine("Folder " + Folder);
                    sw.WriteLine("File " + File);
                    sw.WriteLine("BackgroundFile " + FileBackground);
                    sw.WriteLine("MaxFilesToSave " + MaxFilesToSave);
                    sw.WriteLine("Delay " + Delay);
                    sw.WriteLine("CustomTextColor " + CustomTextColor.ToArgb());
                    sw.WriteLine("CustomGraphColor " + CustomGraphColor.ToArgb());
                }
            }
            catch { }
        }

        /* * Name: load
         * Input: None
         * Output: Void
         * Description: Checks for the existence of the settings file and initiates the reading process.
         */
        public static void load()
        {
            if (!System.IO.File.Exists(settings_path))
                return;

            try
            {
                string[] lines = System.IO.File.ReadAllLines(settings_path);
                foreach (string line in lines)
                {
                    var result = Regex.Split(line, " ");
                    if (result.Length >= 2) Interpret(result);
                }
            }
            catch { }
        }

        /* * Name: Interpret
         * Input: string[] nst (Array containing key and value pairs)
         * Output: Void
         * Description: Parses individual lines from the settings file and updates the corresponding static variables.
         */
        private static void Interpret(string[] nst)
        {
            try
            {
                string key = nst[0];
                string val = nst[1];

                if (key == "window_Height") window_Height = int.Parse(val);
                if (key == "window_Width") window_Width = int.Parse(val);

                if (key == "Gain") Gain = float.Parse(val, CultureInfo.InvariantCulture);
                if (key == "Level") Level = float.Parse(val, CultureInfo.InvariantCulture);
                if (key == "SignalOffset") SignalOffset = float.Parse(val, CultureInfo.InvariantCulture);

                if (key == "Average") Average = int.Parse(val);
                if (key == "Intermediate_average") Intermediate_average = int.Parse(val);
                if (key == "Max_BufferSize") Max_BufferSize = int.Parse(val);

                if (key == "Folder") Folder = val;
                if (key == "File") File = val;
                if (key == "BackgroundFile") FileBackground = val;
                if (key == "MaxFilesToSave") MaxFilesToSave = int.Parse(val);
                if (key == "Delay") Delay = long.Parse(val);

                if (key == "CustomTextColor") CustomTextColor = Color.FromArgb(int.Parse(val));
                if (key == "CustomGraphColor") CustomGraphColor = Color.FromArgb(int.Parse(val));
            }
            catch { }
        }
    }
}