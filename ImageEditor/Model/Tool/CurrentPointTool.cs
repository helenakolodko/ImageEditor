using System.Windows;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public abstract class CurrentPointTool : Tool
    {
        protected CurrentPointTool(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        public override void MouseDown(Point position)
        {
            IsWorking = true;
            ProcessPoint(position);
        }

        public override void MouseMove(Point position)
        {
            if (IsWorking)
            {
                ProcessPoint(position);
            }
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