using ImageEditor.View;
using ImageEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageEditor.Command
{
    class DropboxCommand : IReversableCommand, ICommand
    {
        private ImageEditorViewModel viewModel;

        public DropboxCommand(ImageEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (!viewModel.Dropbox.IsAuthorized)
                viewModel.Dropbox.LogIn();            
            viewModel.Dropbox.ShowDialog();
        }


        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }

        public void RaiseCanExecuteChanged()
        {
        }
    }
}
