using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class RedoCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public RedoCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return _viewModel.ComandList.HasToRedo;
        }

        public void Execute(object parameter)
        {
            _viewModel.Image.Source = _viewModel.ComandList.Redo(_viewModel.Image.Source);
            _viewModel.RefreshImage();
            _viewModel.OnCommandExecuted();
        }

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