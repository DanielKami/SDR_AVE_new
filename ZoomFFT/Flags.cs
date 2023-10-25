using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDRSharp.Average
{
    static class Flags
    {
        //Multiple files save
        public static string Folder;
        public static string File;
        public static int MaxFilesToSave;
        public static long Delay;


        //Window
        public static int window_Height = 400, window_Width = 600;

        public static float Gain = 70;
        public static float Level = 100;
        public static int Average = 10;
        public static int Intermediate_average = 10;
        public static int Max_BufferSize = 1024 / 4;

        //Not saved
        public static int average_max = 50000;
        
        public static int Intermediate_cumulateIndex = 0;
        public static bool background_recording = false;
        public static bool background_corrected = false;
        public static bool background_reset = false;
        private static string settings_path = ".\\IFsettings.txt";


        public static void save()
        {
            // create a writer and open the file
            TextWriter tw = new StreamWriter(settings_path);
            string data_out = "window_Height " + window_Height + "\r\n";
            data_out += "window_Width " + window_Width + "\r\n";
            data_out += "Gain " + Gain + "\r\n";
            data_out += "Level " + Level + "\r\n";
            data_out += "Average " + Average + "\r\n";
            data_out += "Intermediate_average " + Intermediate_average + "\r\n";
            data_out += "Folder " + Folder + "\r\n";
            data_out += "File " + File + "\r\n";
            data_out += "MaxFilesToSave " + MaxFilesToSave + "\r\n";
            data_out += "Delay " + Delay + "\r\n";
            data_out += "Max_BufferSize " + Max_BufferSize + "\r\n";
            


        //write a line of text to the file
            tw.WriteLine(data_out);

            //close the stream
            tw.Close();
        }


        public static void Load()
        {
            if (!System.IO.File.Exists(settings_path))
                return;

            string[] lines = System.IO.File.ReadAllLines(settings_path);


            foreach (string line in lines)
            {
                var result = Regex.Split(line, " ");
                interpret(result);
            }

        }
        public static void interpret(string[] nst)
        {
              if (nst[0] == "window_Height")
                window_Height = int.Parse(nst[1]);

            if (nst[0] == "window_Width")
                window_Width = int.Parse(nst[1]);

            if (nst[0] == "Gain")
                Gain = int.Parse(nst[1]);

            if (nst[0] == "Level")
                Level = int.Parse(nst[1]);

            if (nst[0] == "Average")
                Average = int.Parse(nst[1]);

            if (nst[0] == "Intermediate_average")
                Intermediate_average = int.Parse(nst[1]);

            if (nst[0] == "Folder")
                Folder = nst[1];

            if (nst[0] == "File")
                File = nst[1];

            if (nst[0] == "MaxFilesToSave")
                MaxFilesToSave = int.Parse(nst[1]);

            if (nst[0] == "Delay")
                Delay = int.Parse(nst[1]);

            if (nst[0] == "Max_BufferSize")
                Max_BufferSize = int.Parse(nst[1]);
            

        //else if (nst[0] == "MenuGraphisc")
        //    if (nst[1] == "True") MenuGraphisc = true;
        //    else
        //        MenuGraphisc = false;


        }
    }

}


