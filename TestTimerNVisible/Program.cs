using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
        static void Main(string[] args)
        {
            //Timer tanımlıyoruz.(10000 = 10 saniye)
            Timer t = new Timer(TimerCallback, null, 0, 10000);
            //açık consolu değişkene atıyoruz.
            IntPtr myWindow = GetConsoleWindow();
            //console ekranının saklanmasını sağlıyoruz.
            ShowWindow(myWindow, SW_HIDE);
            Console.ReadLine();
            //console ekranını geri göstermek için kullanılıyor.
            //ShowWindow(handle, SW_SHOW);
        }

        private static void TimerCallback(Object o)
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
