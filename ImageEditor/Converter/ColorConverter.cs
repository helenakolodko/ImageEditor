using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(Color), typeof(System.Windows.Media.Color))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color) value;
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color c = (System.Windows.Media.Color) value;
            return Color.FromArgb(c.R, c.G, c.B);
        }
    }
}