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
            BitmapLock outBitmapLock = new BitmapLock(outBitmap, ImageLockMode.ReadWrite);
            
            //int fullSize = (int)Math.Pow(2*size + 1, 2);

            int N = 2 * size + 1;
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            int c = 5;//параметры фильтра
            //for (double w = 0.1; w < 3; w += 0.05)
            //{
                //Parallel.For(size, image.Width - size-1, x =>
                for (int x = size; x < image.Width - size - 1; x++)
                {
                    for (int y = size; y < image.Height - size - 1; y++)
                    {
                        Color[,] colors = new Color[N, N]; //массив цветов 
                        int[,] r = new int[N, N]; //массив радиусов
                        double[,] h = new double[N, N]; //массив импульсной характеристики пикселей окна
                        double[] H = new double[N]; //массив импульсной характеристики, зависящей от радиуса
                        double resultH = 0;
                        double[,,] convolution = new double[N, N, 3];
                        Color central = Color.Black; //запись центрального отсчёта


                        int left = x - size;
                        int right = x + size;
                        int up = y - size;
                        int down = y + size;


                        double w = 1.5;//параметры фильтра

                        for (int i = left; i <= right; i++)
                        {
                            for (int j = up; j <= down; j++)
                            {
                                int I = i - left, J = j - up;
                                colors[I, J] = image.GetPixel(i, j);
                                if (i == x && j == y)
                                {
                                    central = colors[I, J];
                                }

                                r[I, J] = (int) Math.Sqrt(Math.Pow(I - (N - 1) / 2, 2) +
                                                          Math.Pow(J - (N - 1) / 2, 2)); //вычисление радиуса


                                H[r[I, J]] = Math.Exp(-c * w) / Math.PI *
                                             (Math.Sin(w * r[I, J]) / r[I, J] +
                                              2 * Math.Cos(w * r[I, J]) / (Math.Pow(r[I, J], 2) * w) +
                                              2 * Math.Sin((w * r[I, J])) / (Math.Pow(r[I, J], 3) * Math.Pow(w, 2)) +
                                              (c * Math.Cos(w * r[I, J]) - Math.Sin((w * r[I, J]))) /
                                              (Math.Pow(c, 2) + Math.Pow(r[I, J], 2))); //вычисление импульсного отклика
                                H[0] = 0;

                                h[I, J] = H[r[I, J]]; //передача соответсвующих значений импусльного отклика
                                resultH += h[I, J];
                            }
                        }

                        double norm = 1 / (2 * resultH); //нормирование 
                        //Console.WriteLine(norm);
                        for (int i = left; i <= right; i++)
                        {
                            for (int j = up; j <= down; j++)
                            {
                                int I = i - left, J = j - up;
                                h[I, J] *= norm;
                            }
                        }

                        for (int X = left; X <= right; X++)
                        {
                            for (int Y = up; Y <= down; Y++)
                            {
                                double resultR = 0, resultG = 0, resultB = 0;
                                //double resultR = central.R, resultG = 2*norm*central.G, resultB = 2*norm*central.B;
                                for (int I = X - left; I < N; I++)
                                {
                                    for (int J = Y - up; J < N; J++)
                                    {
                                        resultR += colors[I, J].R * h[I, J];
                                        resultG += colors[I, J].G * h[I, J];
                                        resultB += colors[I, J].B * h[I, J];
                                        Color result = Color.FromArgb(255, (int) resultR, (int) resultG, (int) resultB);
                                        outBitmapLock.SetPixel(X, Y, result);
                                    }
                                }
                                
                            }
                        }
                    }

                } //);
            //}

            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedMilliseconds);
            return outBitmapLock.Release();
            
        }
        
        public static void Main(string[] args)
        {
            Bitmap im = (Bitmap) Image.FromFile("C:/IO/5b.png", false);

            //MedianFilter(im,5).Save("C:/IO/out_ss.jpg");
            SuperFilter(im,1).Save("C:/IO/diploma2.jpg");
        }
    }
}