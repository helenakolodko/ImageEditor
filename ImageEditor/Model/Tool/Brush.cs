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
        private Graphics graphics;
        private Pen pen;
        private Bitmap mask;
        private Graphics maskGraphics;
        private Pen maskPen;
        private List<DrawingPoint> points;
        private Bitmap initialBitmap;
        private Rectangle rectangle;

        public Brush(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void SetStartPoint(Point position)
        {
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            points = new List<DrawingPoint>();
            points.Add(StartPoint);
            Bitmap bitmap = ViewModel.ImageToDisplay.Source;
            initialBitmap = ViewModel.Image.Source;
            rectangle = ViewModel.Image.GetFullRectangle();
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;
            pen.StartCap = LineCap.Round;
            if (ViewModel.Inpainting.CreateMask)
            {
                mask = new Bitmap(ViewModel.ImageWidth, ViewModel.ImageHeight);
                maskGraphics = Graphics.FromImage(mask);
                maskPen = new Pen(Color.White, ViewModel.StrokeThickness);
            }
        }

        protected override void SetEndPoint(Point position)
        {
            graphics.DrawImage(initialBitmap, rectangle, rectangle, GraphicsUnit.Pixel);
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            points.Add(StartPoint);
            graphics.DrawCurve(pen, points.ToArray());
            ViewModel.ImageToDisplay = ViewModel.ImageToDisplay;
        }

        protected override void Finish(Point position)
        {
            StartPoint = new DrawingPoint((int)(position.X / ViewModel.Zoom), (int)(position.Y / ViewModel.Zoom));
            points.Add(StartPoint);
            if (ViewModel.Inpainting.CreateMask)
            {
                maskGraphics.DrawCurve(maskPen, points.ToArray());
                ViewModel.Mask = mask;
            }
            else
            {
                ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
                Bitmap bitmap = ViewModel.Image.Source;
                graphics = Graphics.FromImage(bitmap);
                graphics.DrawCurve(pen, points.ToArray());
                ViewModel.RefreshImage();
                ViewModel.OnCommandExecuted();
            }
        }
    }
}
