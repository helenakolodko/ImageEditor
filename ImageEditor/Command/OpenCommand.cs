using System;
using System.Windows.Input;
using ImageEditor.Properties;
using ImageEditor.ViewModel;
using Microsoft.Win32;

namespace ImageEditor.Command
{
    class OpenCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public OpenCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = Resources.filter };
            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.OpenImage(openFileDialog.FileName);
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
        }
    }
}
