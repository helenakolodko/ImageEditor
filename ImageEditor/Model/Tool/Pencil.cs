using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;

namespace ImageEditor.Model.Tool
{
    class Pencil : DrawingTool
    {
        private Graphics _graphics;
        private Pen _pen;
        private Bitmap _mask;
        private Graphics _maskGraphics;
        private Pen _maskPen;

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
            Bitmap bitmap = ViewModel.Image.Source;
            _graphics = Graphics.FromImage(bitmap);
            _pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            _pen.EndCap = LineCap.Round;
            _pen.StartCap = LineCap.Round;
            if (ViewModel.CreateMask)
            {
                _mask = new Bitmap(ViewModel.ImageWidth, ViewModel.ImageHeight);
                _maskGraphics = Graphics.FromImage(_mask);
                _maskPen = new Pen(Color.White, ViewModel.StrokeThickness);
            }
        }

        protected override void SetEndPoint(Point value)
        {
            EndPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            if (ViewModel.CreateMask)
            {
                _maskGraphics.DrawLine(_maskPen, StartPoint, EndPoint);
                Bitmap bitmap = ViewModel.ImageToDisplay.Source;
                _graphics = Graphics.FromImage(bitmap);
                _graphics.DrawLine(_pen, StartPoint, EndPoint);
                ViewModel.ImageToDisplay = ViewModel.ImageToDisplay;
            }
            else
            {
                _graphics.DrawLine(_pen, StartPoint, EndPoint);   
                ViewModel.RefreshImage();
            }
            
            StartPoint = EndPoint;
        }

        protected override void Finish(Point value)
        {
            SetEndPoint(value);
            ViewModel.Mask = _mask;
            ViewModel.OnCommandExecuted();
        }

        public Pencil(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }
    }
}
