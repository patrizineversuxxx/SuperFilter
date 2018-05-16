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
        private static Bitmap FilterTuning(Bitmap input, Bitmap original, int N, double c)
        {

            double w = 0.1;
            double quality = 0;
            double result = 0;
            while (w <= 3)
            {
                if (quality < MainFilter.PSNR(original, MainFilter.Processing(input, N, c, w)))
                {
                    result = w;
                }

                w += 0.05;
            }

            return MainFilter.Processing(input, N, c, result);
        }
    }
}
