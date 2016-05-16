using System;
using System.Windows.Input;
using ImageEditor.Properties;
using ImageEditor.ViewModel;
using Microsoft.Win32;
using ImageEditor.View;

namespace ImageEditor.Command
{
    class DownloadCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public DownloadCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Dropbox.IsAuthorized;            
        }

        public void Execute(object param)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog { Filter = Resources.filter };
            DownloadDialogWindow dialog = new DownloadDialogWindow();
            if (dialog.OpenDialog() == true)
            {
                _viewModel.OpenImage(dialog.FileName);
            }
            dialog.Close();
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
