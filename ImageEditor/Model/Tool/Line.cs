using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace ImageEditor.Model.Tool
{
    public class Line : DrawingTool
    {
        private readonly Image _imageControl;
        private ColorPicker _colorPicker;
        private Grid _drawinGrid;
        public double Zoom
        {
            private get { return _zoom; }
            set
            {

            }
            
        }

        private Color _penColor
        {
            get { return _colorPicker.SelectedColor; }
        }

        public Line(Image imageControl, ColorPicker colorPicker, Grid drawinGrid)
        {
            _imageControl = imageControl;
            _colorPicker = colorPicker;
            _drawinGrid = drawinGrid;
        }

        public int Thickness { get; set; }

        private DrawingContext _drawingContext;
        private Pen _pen;
        private System.Windows.Shapes.Line _line;

        protected override void SetStartPoint(Point value)
        {
            _startPoint = value;
            this.Thickness = 1;
            _pen = new Pen(new SolidColorBrush(_penColor), Thickness);
            _line = new System.Windows.Shapes.Line
            {
                X1 = _startPoint.X,
                Y1 = _startPoint.Y,
                X2 = _startPoint.X,
                Y2 = _startPoint.Y,
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(_penColor)
            };
            _drawinGrid.Children.Add(_line);
        }

        protected override void SetEndPoint(Point value)
        {
            _line.X2 = value.X;
            _line.Y2 = value.Y;
        }

        protected override void Finish(Point value)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            _drawingContext = drawingVisual.RenderOpen();
            _drawingContext.DrawImage(_imageControl.Source, new Rect(0, 0, _imageControl.Source.Width, _imageControl.Source.Height));
            _drawingContext.DrawLine(_pen, _startPoint, value);
            _drawingContext.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)_imageControl.Source.Width, (int)_imageControl.Source.Height,
                96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            _imageControl.Source = bmp;
            _drawinGrid.Children.Remove(_line);
        }
    }
}
