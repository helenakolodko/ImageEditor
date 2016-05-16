using ImageEditor.View;
using ImageEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace ImageEditor.Command
{
    class UploadCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public UploadCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null && _viewModel.Dropbox.IsAuthorized;
        }

        public void Execute(object param)
        {
            UploadDialogWindow dialog = new UploadDialogWindow() { Image = _viewModel.Image };
            if (dialog.OpenDialog() == true)
            {
               
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
