using System;
using System.Windows.Input;
using ImageEditor.ViewModel;
using ImageProcessing;

namespace ImageEditor.Command
{
    public class HistogramStretchCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public HistogramStretchCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            _viewModel.Image.Source = HistogramEqualazer.Stretch(_viewModel.Image.Source, _viewModel.SelectedRegion);
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