using System.Drawing;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;

namespace ImageEditor.Model.Tool
{
    public class Line : DrawingTool
    {
        private Graphics _graphics;
        private Pen _pen;

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new System.Drawing.Point((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            Bitmap bitmap = ViewModel.Image.Source;
            _graphics = Graphics.FromImage(bitmap);
            _pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
        }

        protected override void SetEndPoint(Point value)
        {

        }

        protected override void Finish(Point value)
        {
            EndPoint = new System.Drawing.Point((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            _graphics.DrawLine(_pen, StartPoint, EndPoint);
        }

        public Line(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void RaiseOnZoomChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}
