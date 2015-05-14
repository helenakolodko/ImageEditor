using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ApplyCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ApplyCommand(ImageEditorViewModel viewModel)
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
            _viewModel.ApplyChanges();
            _viewModel.ResetFields();
        }

        public void Undo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
