using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class ResizeCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ResizeCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object param)
        {
            _viewModel.Image.Resize(_viewModel.ImageWidth, _viewModel.ImageHeight);
            _viewModel.Image = _viewModel.Image;
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

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}