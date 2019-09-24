using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XXXNotas.Utilities
{
    static class StringToColorExtensionMethod
    {
        public static Color ToColor(this string colorString)
        {
            List<byte> rgb = new List<byte>();
            colorString = colorString.Substring(1);
            for(int i = 0; i < colorString.Length; i += 2)
            {
                rgb.Add(byte.Parse(colorString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }
            if(rgb.Count == 3)
            {
                return Color.FromRgb(rgb[0], rgb[1], rgb[2]);
            }
            if(rgb.Count == 4)
            {
                return Color.FromArgb(rgb[0], rgb[1], rgb[2], rgb[3]);
            }
            return Colors.White;
        }


    }
}
