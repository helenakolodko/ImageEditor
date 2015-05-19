using System.Drawing;
using ImageEditor.ViewModel;
using Xceed.Wpf.Toolkit;
using Point = System.Windows.Point;

namespace ImageEditor.Model.Tool
{
    public class Eyedropper : CurrentPointTool
    {
        public Eyedropper(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void ProcessPoint(Point position)
        {
            Bitmap bitmap = ViewModel.ImageToDisplay.Source;
            Color selectedColor = bitmap.GetPixel((int) (position.X/ViewModel.Zoom), (int) (position.Y/ViewModel.Zoom));
            ViewModel.SelectedColor = selectedColor;
        }
    }
}
