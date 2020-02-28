using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TestTimerNVisible
{
    public static class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static int SW_HIDE = 0;
        //static int SW_SHOW = 5;
        static void Main(string[] args)
        {
            // Xd
            Timer t = new Timer(TimerCallback, null, 0, 10000);
            IntPtr myWindow = GetConsoleWindow();
            // Hide
            ShowWindow(myWindow, SW_HIDE);
            Console.ReadLine();
            // Show
            //ShowWindow(handle, SW_SHOW);
        }

        private static void TimerCallback(Object o)
        {
            Console.WriteLine("Timer: " + DateTime.Now);
            GC.Collect();
            WriteToFile("hata");
        }
        private static void WriteToFile(string text)
        {

            string root = @"C:\Log";
            string subdir = @"C:\Log\Log-" + DateTime.Now.ToString("dd/MM/yyyy") + ".txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            if (!Directory.Exists(subdir))
            {
                FileStream fs = new FileStream(subdir, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Close();
            }
            File.AppendAllText(subdir, string.Format(text + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + Environment.NewLine));
        }



    }
}
