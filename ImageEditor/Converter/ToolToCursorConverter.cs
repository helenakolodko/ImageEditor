using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using ImageEditor.Model.Tool;

namespace ImageEditor.Converter
{
    [ValueConversion(typeof(Tool), typeof(Cursor))]
    public class ToolToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tool tool = value as Tool;
            return (tool != null) ? tool.GetCursor() : Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}