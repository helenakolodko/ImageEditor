using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(float), typeof(double))]
    class ContrastConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return .0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (Double.TryParse(value.ToString(), NumberStyles.Any, culture, out result))
            {
                return (float)(Math.Pow(Math.E, (double)(256 + result * 2) / 255) / Math.E);
            }
            return value;
        }
    }
}
