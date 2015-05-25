using System.Windows;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public abstract class Tool
    {
        protected ImageEditorViewModel ViewModel;

        protected Tool(ImageEditorViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected bool IsWorking;

        public virtual Cursor GetCursor()
        {
            return Cursors.Arrow;
        }

        public abstract void MouseDown(Point position);
        public abstract void MouseMove(Point position);
        public abstract void MouseUp(Point position);

    }
}
