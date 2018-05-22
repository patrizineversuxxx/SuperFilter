using System;
using System.Diagnostics;
using System.Drawing;

namespace BaldNeeeeeeeeeer
{
    class Program
    {
        

        static void test(string original, string blurred, string processed)
        {
            Stopwatch watch = Stopwatch.StartNew();
            Tests.FilterTuning(Bitmap.FromFile(original) as Bitmap, Bitmap.FromFile(blurred) as Bitmap, 3).Save(processed);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds / 1000 / 60 + " minutes");
        }

        static void processing(string original, string blurred, string processed)
        {
            Stopwatch watch = Stopwatch.StartNew();
            Bitmap kek = MainFilter.Processing(Bitmap.FromFile(blurred) as Bitmap, 3, 7.95, 1.75);
            Console.WriteLine(MainFilter.PSNR(Bitmap.FromFile(original) as Bitmap, kek));
            kek.Save(processed);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds + " ms");
        }

        static void ShowPSNR(string image1, string image2)
        {
            Console.WriteLine(MainFilter.PSNR(Bitmap.FromFile(image1) as Bitmap, Bitmap.FromFile(image2) as Bitmap));
        }

        static void Main(string[] args)
        {
            string original = "C:/IO/IO/IO/4b.png";
            string blurred = "C:/IO/IO/IO/5a.png";
            string processed = "C:/IO/IO/IO/6a n=3.jpg";

            //ShowPSNR(original, blurred);
            processing(original,blurred,processed);
            //test(original, blurred, processed);
            Console.ReadKey();
        }
    }
}
