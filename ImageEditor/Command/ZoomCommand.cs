using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ZoomCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ZoomCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            if (_viewModel.Image != null)
            {
                return true;
            }
            return false;
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
    }
}
