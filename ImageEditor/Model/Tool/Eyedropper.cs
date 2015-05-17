using ImageEditor.View;
using Xceed.Wpf.Toolkit;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

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
