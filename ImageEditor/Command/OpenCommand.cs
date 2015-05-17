using System;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.Properties;
using ImageEditor.ViewModel;
using Microsoft.Win32;

namespace ImageEditor.Command
{
    class OpenCommand : ICommand
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
                OpenImage(openFileDialog.FileName);
            }
        }

        private void OpenImage(string imagePath)
        {
            _viewModel.Image = new EditableImage(imagePath);
            _viewModel.ResetFields();
            _viewModel.ResetTools();
            _viewModel.Active = true;
            // clear commandList
        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }
    }
}
