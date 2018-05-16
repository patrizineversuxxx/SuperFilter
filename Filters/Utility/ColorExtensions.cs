using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldNeeeeeeeeeer
{
    public static class ColorExtensions
    {
        public static Color Multiply(this Color color, double multiplier)
        {            
            byte r = checked((byte)(color.R * multiplier));
            byte g = checked((byte)(color.G * multiplier));
            byte b = checked((byte)(color.B * multiplier));
            return Color.FromArgb(255, r, g, b);
        }

        public static Color Addition(this Color color, Color other)
        {
            byte r = checked((byte)(color.R + other.R));
            byte g = checked((byte)(color.G + other.G));
            byte b = checked((byte)(color.B + other.B));
            return Color.FromArgb(255, r, g, b);
        }
        public static Color Substraction(this Color color, Color other)
        {
            byte r = checked((byte)(color.R - other.R));
            byte g = checked((byte)(color.G - other.G));
            byte b = checked((byte)(color.B - other.B));
            return Color.FromArgb(255, r, g, b);
        }
    }
}
