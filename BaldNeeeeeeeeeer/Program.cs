using System;
using System.Diagnostics;
using System.Drawing;

namespace BaldNeeeeeeeeeer
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = Stopwatch.StartNew();
            //FilterTuning(Bitmap.FromFile("C:/IO/IO/in.png") as Bitmap, Bitmap.FromFile("C:/IO/IO/in.png") as Bitmap, 7, 5).Save("C:/IO/IO/out9.png");
            GaussianBlur.Processing(Bitmap.FromFile("C:/IO/IO/1in.jpg") as Bitmap).Save("C:/IO/IO/12Gauss.jpg");
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
