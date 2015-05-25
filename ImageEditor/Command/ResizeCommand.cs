using System;
using System.Drawing;
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
            _viewModel.ComandList.AddNew(new Bitmap(_viewModel.Image.Source));
            _viewModel.Image.Resize(_viewModel.ImageWidth, _viewModel.ImageHeight);
            _viewModel.RefreshImage();
            _viewModel.OnCommandExecuted();
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