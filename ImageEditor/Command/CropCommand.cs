using System;
using System.Windows.Input;
using ImageEditor.Model.Tool;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class CropCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public CropCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null && _viewModel.SelectedTool is Selection;
        }

        public void Execute(object parameter)
        {
            _viewModel.ResetFields();
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

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null) 
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
