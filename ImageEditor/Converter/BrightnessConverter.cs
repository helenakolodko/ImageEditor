using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(float), typeof(double))]
    class BrightnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (float) value;
            return Math.Round(v * 255, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (Double.TryParse(value.ToString(), NumberStyles.Any, culture, out result))
            {
                return result / 255f;
            }
            return value;
        }
    }
}
