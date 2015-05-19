using System.Windows;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public abstract class FixedPointsTool : Tool
    {
        protected Point StartPoint;
        protected Point EndPoint;
        protected FixedPointsTool(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void MouseDown(Point position)
        {
            IsWorking = true;
            SetStartPoint(position);
        }

        protected abstract void SetStartPoint(Point position);

        public override void MouseMove(Point position)
        {
            if (IsWorking)
            {
                SetEndPoint(position);
            }
        }

        protected abstract void SetEndPoint(Point position);

        public override void MouseUp(Point position)
        {
            if (IsWorking)
            {
                SetEndPoint(position);
                IsWorking = false;
            }
        }
    }
}