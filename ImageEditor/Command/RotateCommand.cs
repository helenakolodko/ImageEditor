using System;
using System.Drawing;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class RotateCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public RotateCommand(ImageEditorViewModel viewModel)
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
            string direction = (string)param;
            if (direction == "Clockwise")
            {
                _viewModel.Image.RotateClockwise();
            }
            else
            {
                _viewModel.Image.RotateAntiClockwise();
            }
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