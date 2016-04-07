using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ApplyCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel viewModel;

        public ApplyCommand(ImageEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.Image != null;
        }

        public void Execute(object parameter)
        {
            viewModel.ComandList.AddNew(viewModel.Image.Source);
            viewModel.ApplyChanges();
            viewModel.ResetFields();
            viewModel.OnCommandExecuted();
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
