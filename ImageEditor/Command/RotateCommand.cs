using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class RotateCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public RotateCommand(ImageEditorViewModel viewModel)
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
            string direction = (string)param;
            if (direction == "Clockwise")
            {
                _viewModel.Image.RotateClockwise();
            }
            else
            {
                _viewModel.Image.RotateAntiClockwise();
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}