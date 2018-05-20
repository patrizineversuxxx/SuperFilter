using System;
using System.Diagnostics;
using System.Drawing;

namespace BaldNeeeeeeeeeer
{
    class Program
    {
        static void Main(string[] args)
        {
            string original = "C:/IO/IO/IO/4b.png";
            string blurred = "C:/IO/IO/IO/5a.png";
            string processed = "C:/IO/IO/IO/6a.jpg";
            Stopwatch watch = Stopwatch.StartNew();
            //FilterTuning(Bitmap.FromFile("C:/IO/IO/in.png") as Bitmap, Bitmap.FromFile("C:/IO/IO/in.png") as Bitmap, 7, 5).Save("C:/IO/IO/out9.png");
            //GaussianBlur.Processing(Bitmap.FromFile("C:/IO/IO/1in.jpg") as Bitmap).Save("C:/IO/IO/out.jpg");
            Tests.FilterTuning(Bitmap.FromFile(original) as Bitmap, Bitmap.FromFile(blurred) as Bitmap, 7).Save(processed);
            //MainFilter.Processing(Bitmap.FromFile(blurred) as Bitmap, 5, 7.95, 1.75).Save(processed);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds/1000/60+" minutes");
            Console.ReadKey();
        }
    }
}
