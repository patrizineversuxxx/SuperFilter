using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Graphics;

namespace SuperFilter
{
    internal class Program
    {
        public static Bitmap MedianFilter(Bitmap input, int size)
        {
            BitmapLock image = new BitmapLock(input, ImageLockMode.ReadOnly);
            Bitmap outBitmap = new Bitmap(image.Width, image.Height);
            BitmapLock outBitmapLock = new BitmapLock(outBitmap, ImageLockMode.WriteOnly);
            
            int fullSize = (int)Math.Pow(2*size + 1, 2);
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            Parallel.For(size, image.Width - size-1, x =>
            {
                for(int y=size; y<image.Height-size-1; y++)
                {
                    Color[] colors = new Color[fullSize];
                    
                    int left = x - size;
                    int right = x + size;
                    int up = y - size;
                    int down = y + size;
                    int n = 0;
                    
                    for (int i = left; i <= right; i++)
                    {
                        for (int j = up; j <= down; j++)
                        {
                            colors[n] = image.GetPixel(i, j);
                            n++;
                        }
                    }

                    Color result = Color.FromArgb(
                        255,
                        colors.OrderBy(c => c.R).ElementAt(fullSize / 2).R,
                        colors.OrderBy(c => c.G).ElementAt(fullSize / 2).G,
                        colors.OrderBy(c => c.B).ElementAt(fullSize / 2).B
                    );

                    outBitmapLock.SetPixel(x, y, result);
                }
                
            });
            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedMilliseconds);
            return outBitmapLock.Release();
        }

        public static Bitmap SuperFilter(Bitmap input, int size)
        {
            BitmapLock image = new BitmapLock(input, ImageLockMode.ReadOnly);
            Bitmap outBitmap = new Bitmap(image.Width, image.Height);
            BitmapLock outBitmapLock = new BitmapLock(outBitmap, ImageLockMode.WriteOnly);
            
            int fullSize = (int)Math.Pow(2*size + 1, 2);

            int N = 2 * size + 1;
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            Parallel.For(size, image.Width - size-1, x =>
            {
                for(int y=size; y<image.Height-size-1; y++)
                {
                    Color[] colors = new Color[fullSize];
                    int [,] r = new int[N,N];
                    double [,] h = new double[image.Width,image.Height];
                    double[] H = new double[size];
                    
                    int left = x - size;
                    int right = x + size;
                    int up = y - size;
                    int down = y + size;
                    int n = 0;

                    int c = 5;
                    double w = 1;
                    
                    for (int i = left; i <= right; i++)
                    {
                        for (int j = up; j <= down; j++)
                        {
                            colors[n] = image.GetPixel(i, j);
                            r[i, j] = (int)Math.Sqrt(Math.Pow(i - (N + 1) / 2, 2) + Math.Pow(j - (N + 1) / 2, 2));
                            
                            H[r[i,j]] = Math.Exp(-c * w) / Math.PI *
                                      (Math.Sin(w * r[i, j]) / r[i, j] +
                                       2 * Math.Cos(w * r[i, j]) / (Math.Pow(r[i, j], 2) * w) +
                                       2 * Math.Sin((w * r[i, j])) / (Math.Pow(r[i, j], 3) * Math.Pow(w, 2)) +
                                       (c * Math.Cos(w * r[i, j]) - Math.Sin((w * r[i, j]))) /
                                       (Math.Pow(c, 2) + Math.Pow(r[i, j], 2)));
                            h[i, j] = H[r[i,j]];
                            h[x, y] += h[i, j];
                        }
                    }
                    double keek = h[x,y]-h[left+size,up+size];
                    h[x,y]*=1 / (2 * keek);
                    Color result = Color.FromArgb(255,(int)h[x,y]*colors[fullSize/2].R,(int)h[x,y]*colors[fullSize/2].G, (int)h[x,y]*colors[fullSize/2].B);

                    outBitmapLock.SetPixel(x, y, result);
                }
                
            });
            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedMilliseconds);
            return outBitmapLock.Release();
        }
        
        public static void Main(string[] args)
        {
            Bitmap im = (Bitmap) Image.FromFile("C:/IO/4b.png", false);

            MedianFilter(im,5).Save("C:/IO/out_ss.jpg");
            SuperFilter(im,3).Save("C:/IO/diploma.jpg");
            
            //Console.ReadKey();
        }
    }
}