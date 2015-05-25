using System.Windows;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public abstract class EndPointTool: Tool
    {
        protected EndPointTool(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void MouseDown(Point position)
        {
            IsWorking = true;
        }

        public override void MouseMove(Point position)
        {
        }

        public override void MouseUp(Point position)
        {
            if (IsWorking)
            {
                ProcessPoint(position);
                IsWorking = false;
            }
        }

        protected abstract void ProcessPoint(Point position);
    }
}