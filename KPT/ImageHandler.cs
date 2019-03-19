using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace KPT
{
    /// <summary>
    /// Class for converting images from GIM to PNG and vice versa
    /// </summary>
    static class ImageHandler
    {

        public const string imagesDir = "Images";
        public static string targetDir;
        public static string gimConvPath;
        public static bool initalized = false;

        // Direction of conversion (i.e. GIM to PNG or PNG to GIM) is decided by input/output file extensions
        public static bool ConvertImage(string inputFilePath, string outputFilePath)
        {

            if (!initalized)
            {
                Initalize();
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = gimConvPath;
            startInfo.Arguments = string.Format("\"{0}\" -o \"{1}\"", inputFilePath, outputFilePath);
            var process = Process.Start(startInfo);
            process.WaitForExit();

            // This will most likely get very very annoying if there are multiple failures - not sure how to handle it. Maybe write to a log file and inform the user of the first failure only?
            if (process.ExitCode != 0)
            {
                //string errorMessage =
                //    "Image conversion failed" + Environment.NewLine + Environment.NewLine +
                //    "Exit code: " + process.ExitCode.ToString() + Environment.NewLine +
                //    "GimConv Path: " + startInfo.FileName + Environment.NewLine +
                //    "Arguments: " + startInfo.Arguments;
                //MessageBox.Show(errorMessage);
                return false;
            }

            return true;
        }

        public static string GetImagesDir()
        {

            if (!initalized)
            {
                Initalize();
            }

            return targetDir;
        }

        private static void Initalize()
        {
            targetDir = Path.Combine(ProjectFolder.GetRootDir(), ProjectFolder.editableGameFiesDir, imagesDir);
            gimConvPath = Path.Combine(ProjectFolder.toolsDir, "GimConv\\GimConv.exe");

            if (!File.Exists(gimConvPath))
            {
                string errorMessage = "Could not find GimConv.exe - expected path was " + gimConvPath;
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }



            initalized = true;

        }


    }
}
