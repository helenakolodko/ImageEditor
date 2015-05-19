using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ZoomCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ZoomCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object param)
        {
            string direction = (string) param;
            if (direction == "In")
            {
                _viewModel.Zoom += .25f;
            }
            else
            {
                _viewModel.Zoom -= _viewModel.Zoom > .25f ? .25f : .1f;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
