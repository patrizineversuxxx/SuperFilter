using Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldNeeeeeeeeeer
{
    class MainFilter
    {
        private static double max(double[] array)
        {
            double result = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (result < array[i])
                    result = array[i];
            }
            return result;
        }

        public static double PSNR(Bitmap original, Bitmap processed)
        {
            BitmapLock originalBitmapLock = new BitmapLock(original, ImageLockMode.ReadOnly);
            BitmapLock processedBitmapLock = new BitmapLock(processed, ImageLockMode.ReadOnly);

            double[] result = { 0, 0, 0 };
            Parallel.For(0, original.Width, i =>
            {
                for (int j = 0; j < original.Height; j++)
                {
                    result[0] += Math.Pow(originalBitmapLock.GetPixel(i, j).R - processedBitmapLock.GetPixel(i, j).R, 2);
                    result[1] += Math.Pow(originalBitmapLock.GetPixel(i, j).G - processedBitmapLock.GetPixel(i, j).G, 2);
                    result[2] += Math.Pow(originalBitmapLock.GetPixel(i, j).B - processedBitmapLock.GetPixel(i, j).B, 2);
                }
            });
            result[0] = result[0] / (original.Width * original.Height);
            result[1] = result[1] / (original.Width * original.Height);
            result[2] = result[2] / (original.Width * original.Height);
            double MSE = max(result);
            double PSNR = 10 * Math.Log10(Math.Pow(255, 2) / MSE);
            return PSNR;
        }

        public static Bitmap Processing(Bitmap input, int N, double c, double w)
        {
            BitmapLock inputBitmapLock = new BitmapLock(input, ImageLockMode.ReadOnly);
            Bitmap output = new Bitmap(input.Width, input.Height);
            BitmapLock outBitmapLock = new BitmapLock(output, ImageLockMode.WriteOnly);
            Parallel.For(N / 2, inputBitmapLock.Width - N / 2, x =>
            {
                for (int y = N / 2; y < inputBitmapLock.Height - N / 2; y++)
                {
                    double[,] h = new double[N, N];
                    double[,] hn = new double[N, N];
                    double b = 0;

                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            double r = Math.Sqrt(Math.Pow(i - N / 2, 2) + Math.Pow(j - N / 2, 2));
                            if (r > 0.5)
                            {
                                h[i, j] = Math.Exp(-c * w) * (Math.Sin(w * r) / r +
                                                              2 * Math.Cos(w * r) / (Math.Pow(r, 2) * w) -
                                                              2 * Math.Sin(w * r) / (Math.Pow(r, 3) * Math.Pow(w, 2)) +
                                                              (c * Math.Cos(w * r) - r * Math.Sin(w * r)) /
                                                              (Math.Pow(c, 2) + Math.Pow(r, 2)));
                            }
                            b += h[i, j];
                        }
                    }

                    h[N / 2, N / 2] = b;

                    double n = 1 / (2 * b);
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            hn[i, j] = n * h[i, j];
                        }
                    }

                    int R = 0, G = 0, B = 0;
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            int I = x + i - N / 2;
                            int J = y + j - N / 2;
                            var color = inputBitmapLock.GetPixel(I, J);


                            R += (int)(color.R * hn[i, j]);
                            G += (int)(color.G * hn[i, j]);
                            B += (int)(color.B * hn[i, j]);
                        }
                    }

                    outBitmapLock.SetPixel(x, y, Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
                }
            });

            return outBitmapLock.Release();
        }

    }
}
