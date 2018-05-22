using Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldNeeeeeeeeeer
{
    class Tests
    {
        public static Bitmap FilterTuning( Bitmap original, Bitmap blurred, int N)
        {
            double c = 7.95;
            double w = 0.1;
            double prev = 0, cur = 0;
            double result_w = 0;
            double result_c=0;
            Bitmap kek;
            while (w <= 3)
            {
                //c = 5;
                //while (c < 9)
                //{
                    kek = MainFilter.Processing(blurred, N, c, w);
                    cur = MainFilter.PSNR(kek, original);
                    if (prev < cur)
                    {
                        prev = cur;
                        result_c = c;
                        result_w = w;
                        
                        Console.WriteLine(w + "  " + result_w + "c =" + c);
                    }

                    //c += 1;
                    //string result = "C:/IO/IO/IO/IO/" + w + " " + c + ".png";
                    //kek.Save(result);
                //}
                w += 0.025;

            }
            
            Console.WriteLine(result_w + "  " + result_c + " || "+ prev + "   " + cur);

            return MainFilter.Processing(blurred, N, result_c, result_w);
        }
    }
}
