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
            double c = 1;
            double w = 0.1;
            double prev = 0, cur = 0;
            double result_w = 0;
            double result_c=0;
            while (w <= 3)
            {
                c = 1;
                while (c < 9)
                {
                    cur = MainFilter.PSNR(MainFilter.Processing(blurred, N, c, w), original);
                    if (prev < cur)
                    {
                        prev = cur;
                        result_c = c;
                        result_w = w;
                        
                        
                        Console.WriteLine(w + "  " + result_w + "c =" + c);
                    }

                    c += 1;
                }
                w += 0.1;
            }
            
            Console.WriteLine(result_w + "  " + result_c + " || "+ prev + "   " + cur);

            return MainFilter.Processing(blurred, N, result_c, result_w);
        }
    }
}
