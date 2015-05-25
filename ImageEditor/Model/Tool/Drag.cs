using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    class Drag : FixedPointsTool
    {
        public Drag(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        private void MoveImage()
        {
            ViewModel.HorizontalOffset += StartPoint.X - EndPoint.X;
            ViewModel.VerticalOffset  += StartPoint.Y - EndPoint.Y;
        }

        protected override void SetStartPoint(Point position)
        {
            StartPoint = position;
        }

        protected override void SetEndPoint(Point position)
        {
            EndPoint = position;
            MoveImage();
        }

        public override Cursor GetCursor()
        {
            return Cursors.Hand;
        }
    }
}
