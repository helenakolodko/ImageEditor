using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class FlipCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public FlipCommand(ImageEditorViewModel viewModel)
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

        public void Execute(object parameter)
        {
            string orientation = (string)parameter;
            if (orientation == "H")
            {
                _viewModel.Image.FlipHorisontal();
            }
            else
            {
                _viewModel.Image.FlipVertical();
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