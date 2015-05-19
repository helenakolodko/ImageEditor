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

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            Bitmap bitmap = ViewModel.Image.Source;
            _graphics = Graphics.FromImage(bitmap);
            _pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            _pen.EndCap = LineCap.Round;
            _pen.StartCap = LineCap.Round;
        }

        protected override void SetEndPoint(Point value)
        {
            EndPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            _graphics.DrawLine(_pen, StartPoint, EndPoint);
            StartPoint = EndPoint;
            ViewModel.Image = ViewModel.Image;
        }

        protected override void Finish(Point value)
        {
            SetEndPoint(value);
        }

        public Pencil(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void RaiseOnZoomChanged()
        {
        }
    }
}
