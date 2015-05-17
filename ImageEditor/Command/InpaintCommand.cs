using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class InpaintCommand :ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public InpaintCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param)
        {
            throw new NotImplementedException();
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