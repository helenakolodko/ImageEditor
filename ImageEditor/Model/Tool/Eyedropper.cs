using System.Drawing;
using Xceed.Wpf.Toolkit;

namespace ImageEditor.Model.Tool
{
    class Eyedropper : SelectTool
    {
        private readonly EditableImage _image;
        private Color _selectedColor;
        private ColorPicker _colorPicker;
        public double Zoom
        {
            private get { return _zoom; }
            set
            {
                
            }
        }

        public Eyedropper(EditableImage image, ColorPicker colorPicker)
        {
            _image = image;
            _colorPicker = colorPicker;
        }

        public Color GetColor()
        {
            return _selectedColor;
//            var b = (Bitmap)_image;
//            Color c = b.GetPixel((int)point.X, (int)point.Y);
//            ColorPicker.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        protected Point StartPoint
        {
            set
            {
               
            }
        }

        protected Point EndPoint
        {
            set
            {
                
            }
        }

    }
}
