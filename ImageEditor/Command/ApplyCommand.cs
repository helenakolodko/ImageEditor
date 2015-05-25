using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ApplyCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ApplyCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object parameter)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            _viewModel.ApplyChanges();
            _viewModel.ResetFields();
            _viewModel.OnCommandExecuted();
        }

        public void Undo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}
