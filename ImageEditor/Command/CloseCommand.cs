using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class CloseCommand : IReversableCommand, ICommand
    {
         private readonly ImageEditorViewModel _viewModel;

        public CloseCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // ???????????????
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
        }

        public event EventHandler CanExecuteChanged;
    }
}