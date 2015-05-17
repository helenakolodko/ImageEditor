using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ImageEditor.Model.Tool
{
    class Pen : DrawingTool
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

        private System.Windows.Media.Color _penColor
        {
            get { return _colorPicker.SelectedColor; }
        }

        public Pen(Image imageControl, ColorPicker colorPicker, Grid drawinGrid)
        {
            _imageControl = imageControl;
            _colorPicker = colorPicker;
            _drawinGrid = drawinGrid;
        }

        public int Thickness { get; set; }

        private DrawingContext _drawingContext;
        private DrawingVisual _drawingVisual;
        private PathGeometry _pathGeometry;
        protected PathFigure _pathFigure;
        protected Path _path;
        protected System.Windows.Media.Pen _pen;

        protected override void SetStartPoint(Point value)
        {
            _startPoint = value;
            this.Thickness = 1;
            _pen = new System.Windows.Media.Pen(new SolidColorBrush(_penColor), Thickness);
            _pathGeometry = new PathGeometry {FillRule = FillRule.Nonzero};
            _pathFigure = new PathFigure
            {
                StartPoint = value,
                IsClosed = false
            };
            _pathGeometry.Figures.Add(_pathFigure);
            _path = new Path
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(_penColor),
                StrokeThickness = Thickness,
                Data = _pathGeometry
            };
            _drawinGrid.Children.Add(_path);
        }

        protected override void SetEndPoint(Point value)
        {
            LineSegment lineSegment = new LineSegment {Point = value};
            _pathFigure.Segments.Add(lineSegment);   
        }

        protected override void Finish(Point value)
        {
            _drawingVisual = new DrawingVisual();
            _drawingContext = _drawingVisual.RenderOpen();
            _drawingContext.DrawImage(_imageControl.Source, new Rect(0, 0, _imageControl.Source.Width, _imageControl.Source.Height));
            _drawingContext.DrawGeometry(new SolidColorBrush(), _pen, _pathGeometry);
            _drawingContext.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)_imageControl.Source.Width, (int)_imageControl.Source.Height,
                96, 96, PixelFormats.Pbgra32);
            bmp.Render(_drawingVisual);
            _imageControl.Source = bmp;
            _drawinGrid.Children.Remove(_path);
        }

    }
}
