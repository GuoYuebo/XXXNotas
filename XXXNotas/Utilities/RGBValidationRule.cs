using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XXXNotas.Utilities
{
    class RgbValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string rgb = value.ToString();
            int length = rgb.Length;
            const string valid = "1234567890abcdef";

            if(length > 0)
            {
                if(rgb[0] == '#')
                {
                    foreach(char c in rgb.Substring(1))
                    {
                        if(valid.IndexOf(char.ToLower(c)) == -1)
                        {
                            return new ValidationResult(false, Resources.Strings.BadRGB);
                        }
                    }
                }
                else
                {
                    return new ValidationResult(false, Resources.Strings.BadRGB);
                }
            }

            if(length != 4 && length != 5 && length != 7 && length != 9)
            {
                return new ValidationResult(false, Resources.Strings.BadRGB);
            }

            return new ValidationResult(true, null);
        }
    }
}
