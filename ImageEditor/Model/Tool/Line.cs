using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Input;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;

namespace ImageEditor.Model.Tool
{
    public class Line : DrawingTool
    {
        private Graphics _graphics;
        private Pen _pen;
        private Bitmap _initialBitmap;
        private Rectangle _rectangle;

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            Bitmap bitmap = ViewModel.ImageToDisplay.Source;
            _initialBitmap = ViewModel.Image.Source;
            _rectangle = ViewModel.Image.GetFullRectangle();
            _graphics = Graphics.FromImage(bitmap);
            _pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            _pen.EndCap = LineCap.Round;
            _pen.StartCap = LineCap.Round;
        }

        protected override void SetEndPoint(Point value)
        {
            _graphics.DrawImage(_initialBitmap, _rectangle, _rectangle, GraphicsUnit.Pixel);
            EndPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            _graphics.DrawLine(_pen, StartPoint, EndPoint);
            ViewModel.ImageToDisplay = ViewModel.ImageToDisplay;
        }

        protected override void Finish(Point value)
        {
            ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
            _graphics.DrawImage(_initialBitmap, _rectangle, _rectangle, GraphicsUnit.Pixel);
            EndPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            Bitmap bitmap = ViewModel.Image.Source;
            _graphics = Graphics.FromImage(bitmap);
            _graphics.DrawLine(_pen, StartPoint, EndPoint);
            ViewModel.RefreshImage();
            ViewModel.OnCommandExecuted();
        }

        public override Cursor GetCursor()
        {
            return Cursors.Cross;
        }

        public Line(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }
    }
}
