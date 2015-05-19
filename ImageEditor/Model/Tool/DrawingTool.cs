using System.Windows;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public abstract class DrawingTool : FixedPointsTool
    {
        protected System.Drawing.Point StartPoint;
        protected System.Drawing.Point EndPoint;
        protected DrawingTool(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void MouseUp(Point position)
        {
            if (IsWorking)
            {
                Finish(position);
                IsWorking = false;
            }
        }

        public override Cursor GetCursor()
        {
            return Cursors.Pen;
        }

        protected abstract void Finish(Point position);
    }
}