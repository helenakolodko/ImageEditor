using System;
using System.Windows.Data;
using Microsoft.Expression.Utility.Extensions.Math;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(double), typeof(string))]
    public class ZoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return ((double) value * 100).RoundToSignificantDigits(1) + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double result;
            if (Double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result / 100;
            }
            else if (Double.TryParse(value.ToString().Replace("%", ""), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result / 100;
            }
            return value;
        }
    }
}
