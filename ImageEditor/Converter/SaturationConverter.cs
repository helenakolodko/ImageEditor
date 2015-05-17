using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(float), typeof(double))]
    class SaturationConverter : IValueConverter
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
                if (result > 0)
                {
                    result *= 10;
                }
                return (float)(255 + result) / 255;
            }
            return value;
        }
    }
}
