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
        private Graphics graphics;
        private Pen pen;
        private Bitmap mask;
        private Graphics maskGraphics;
        private Pen maskPen;

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
            Bitmap bitmap = ViewModel.Image.Source;
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(ViewModel.SelectedColor, ViewModel.StrokeThickness);
            pen.EndCap = LineCap.Round;
            pen.StartCap = LineCap.Round;
            if (ViewModel.Inpainting.CreateMask)
            {
                mask = new Bitmap(ViewModel.ImageWidth, ViewModel.ImageHeight);
                maskGraphics = Graphics.FromImage(mask);
                maskPen = new Pen(Color.White, ViewModel.StrokeThickness);
            }
        }

        protected override void SetEndPoint(Point value)
        {
            EndPoint = new DrawingPoint((int)(value.X / ViewModel.Zoom), (int)(value.Y / ViewModel.Zoom));
            if (ViewModel.Inpainting.CreateMask)
            {
                maskGraphics.DrawLine(maskPen, StartPoint, EndPoint);
                Bitmap bitmap = ViewModel.ImageToDisplay.Source;
                graphics = Graphics.FromImage(bitmap);
                graphics.DrawLine(pen, StartPoint, EndPoint);
                ViewModel.ImageToDisplay = ViewModel.ImageToDisplay;
            }
            else
            {
                graphics.DrawLine(pen, StartPoint, EndPoint);   
                ViewModel.RefreshImage();
            }
            
            StartPoint = EndPoint;
        }

        protected override void Finish(Point value)
        {
            SetEndPoint(value);
            ViewModel.Mask = mask;
            ViewModel.OnCommandExecuted();
        }

        public Pencil(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }
    }
}
