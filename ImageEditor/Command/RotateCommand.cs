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
            return true;
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
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}