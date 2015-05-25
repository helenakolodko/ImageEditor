using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;

namespace ImageEditor.Model.Tool
{
    internal class Brush : DrawingTool
    {
        private Graphics _graphics;
        private Pen _pen;
        private Bitmap _mask;
        private Graphics _maskGraphics;
        private Pen _maskPen;
        private List<DrawingPoint> _points;
        private Bitmap _initialBitmap;
        private Rectangle _rectangle;

        public Brush(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void SetStartPoint(Point position)
        {
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            _points = new List<DrawingPoint>();
            _points.Add(StartPoint);
            Bitmap bitmap = ViewModel.ImageToDisplay.Source;
            _initialBitmap = ViewModel.Image.Source;
            _rectangle = ViewModel.Image.GetFullRectangle();
            _graphics = Graphics.FromImage(bitmap);
            _pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            _pen.EndCap = LineCap.Round;
            _pen.LineJoin = LineJoin.Round;
            _pen.StartCap = LineCap.Round;
            if (ViewModel.CreateMask)
            {
                _mask = new Bitmap(ViewModel.ImageWidth, ViewModel.ImageHeight);
                _maskGraphics = Graphics.FromImage(_mask);
                _maskPen = new Pen(Color.White, ViewModel.StrokeThickness);
            }
        }

        protected override void SetEndPoint(Point position)
        {
            _graphics.DrawImage(_initialBitmap, _rectangle, _rectangle, GraphicsUnit.Pixel);
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            _points.Add(StartPoint);
            _graphics.DrawCurve(_pen, _points.ToArray());
            ViewModel.ImageToDisplay = ViewModel.ImageToDisplay;
        }

        protected override void Finish(Point position)
        {
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            _points.Add(StartPoint);
            if (ViewModel.CreateMask)
            {
                _maskGraphics.DrawCurve(_maskPen, _points.ToArray());
                ViewModel.Mask = _mask;
            }
            else
            {
                ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
                Bitmap bitmap = ViewModel.Image.Source;
                _graphics = Graphics.FromImage(bitmap);
                _graphics.DrawCurve(_pen, _points.ToArray());
                ViewModel.RefreshImage();
                ViewModel.OnCommandExecuted();
            }
        }
    }
}
