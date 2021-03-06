﻿using Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using BaldNeeeeeeeeeer;
using System.Linq;

public static class GaussianBlur
{
    private static double[] window;

    private static double getWindowValue(int index)
    {
        if (index < 0)
            return window[-index];
        return window[index];
    }

    public static Bitmap Processing(Bitmap inputImage)
    {
        BitmapLock input = new BitmapLock(inputImage, ImageLockMode.ReadOnly);
        int N = 13;
        int maskSize = N / 2;
        window = new double[maskSize];

        Bitmap outputImage = new Bitmap(input.Width, input.Height);
        BitmapLock output = new BitmapLock(outputImage, ImageLockMode.WriteOnly);
        double b = 0;
        for (int i = 0; i < maskSize; i++)
        {
            window[i] = Math.Exp(-Math.Pow(i, 2) / (2 * Math.Pow(maskSize, 2)))
                / Math.Sqrt(2 * Math.PI * Math.Pow(2, 2));

            b += window[i];
        }
        double kek = window[0];
        for (int i = 0; i < maskSize; i++)
        {
            window[i] = window[i]/(b*4-2*kek);
        }

        Parallel.For(0, input.Width -1 , x =>
        {
            for (int y = 0; y < input.Height - 1; y++)
            {

                if (x < maskSize || x > input.Width - maskSize || y < maskSize || y > input.Height - maskSize)
                {
                    output.SetPixel(x, y, input.GetPixel(x, y));
                }
                else
                {
                    Color result = Color.FromArgb(255, 0, 0, 0);

                    for (int i = -maskSize+1; i < maskSize; i++)
                    {
                        Color cec = input.GetPixel(x + i, y);
                        result = result.Addition(cec.Multiply(getWindowValue(i)));
                    }

                    for (int i = -maskSize + 1; i < maskSize; i++)
                    {
                        Color cec = input.GetPixel(x, y + i);
                        result = result.Addition(cec.Multiply(getWindowValue(i)));
                    }

                    output.SetPixel(x, y, result);
                }
            }
        });

        return output.Release();
    }
}