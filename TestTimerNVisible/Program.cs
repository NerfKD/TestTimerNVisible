using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Timers;

namespace TestTimerNVisible
{
    public static class Program
    {
        //Console u saklamak için DLL'leri ımport edip tanımlamaları yapıyoruz.
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static int SW_HIDE = 0;
        //static int SW_SHOW = 5;

        public static System.Timers.Timer aTimer;

        static void Main(string[] args)
        {
            //Timer tanımlıyoruz.(10000 = 10 saniye)
            SetTimer();
            //System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Stop();
            //açık consolu değişkene atıyoruz.
            IntPtr myWindow = GetConsoleWindow();
            //console ekranının saklanmasını sağlıyoruz.
            ShowWindow(myWindow, SW_HIDE);

            //console ekranını geri göstermek için kullanılıyor.
            //ShowWindow(handle, SW_SHOW);
            Thread.Sleep(10000);
            aTimer.Start();
            Console.ReadLine();

        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //timer sonucunu console yazdırmamızı sağlıyor.
            Console.WriteLine("Timer: " + DateTime.Now);
            GC.Collect();
            //hata mesajını metoda göndererek çalıştıyoruz
            WriteToFile("hata");
        }
        private static void WriteToFile(string text)
        {
            //Log klasörünün yollarını belirtiyoruz.
            string root = @"C:\Log";
            string subdir = @"C:\Log\Log-" + DateTime.Now.ToString("dd/MM/yyyy") + ".txt";

            //belirtilen yolda Log klasörü yoksa oluştur.
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            //belirtilen yolda  log.txt yoksa oluştur.
            if (!Directory.Exists(subdir))
            {
                FileStream fs = new FileStream(subdir, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Close();
            }
            //oluşturulmuş olan log.txt ye satır satır mesajı ve tarih saati yazdır.
            File.AppendAllText(subdir, string.Format(text + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + Environment.NewLine));
        }

    }
}
