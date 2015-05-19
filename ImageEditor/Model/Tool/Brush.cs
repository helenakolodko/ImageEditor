using System.Windows;
using System.Windows.Controls;
using ImageEditor.ViewModel;
using Xceed.Wpf.Toolkit;

namespace ImageEditor.Model.Tool
{
    internal class Brush : Pencil
    {

        protected override void SetStartPoint(Point value)
        {
           base.SetStartPoint(value);
//            _pen.Thickness *= 3;
        }

        public Brush(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }
    }
}
