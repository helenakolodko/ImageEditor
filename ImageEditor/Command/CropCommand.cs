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
            return _viewModel.Image != null && _viewModel.Selection.Active;
        }

        public void Execute(object parameter)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            _viewModel.ResetFields();
            _viewModel.Image.Crop(_viewModel.SelectedRegion);
            _viewModel.RefreshImage();
            _viewModel.ResetTools();
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
