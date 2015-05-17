using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class ResizeCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ResizeCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param)
        {
            _viewModel.Image.Resize(_viewModel.ImageWidth, _viewModel.ImageHeight);
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