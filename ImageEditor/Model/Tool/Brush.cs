using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace ImageEditor.Model.Tool
{
    internal class Brush : Pencil
    {
        public Brush(Image imageControl, ColorPicker colorPicker, Grid drawinGrid) 
            : base(imageControl, colorPicker, drawinGrid)
        {
        }

        protected override void SetStartPoint(Point value)
        {
            base.SetStartPoint(value);
            _pen.Thickness *= 3;
            _path.StrokeThickness *= 3;
        }
    }
}
