using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Expression.Utility.Extensions.Math;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(double), typeof(string))]
    public class ZoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double) value * 100).RoundToSignificantDigits(3) + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (Double.TryParse(value.ToString(), NumberStyles.Any, culture, out result))
            {
                return result / 100;
            }
            else if (Double.TryParse(value.ToString().Replace("%", ""), NumberStyles.Any, culture, out result))
            {
                return result / 100;
            }
            return value;
        }
    }
}
