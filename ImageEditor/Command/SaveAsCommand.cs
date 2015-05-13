using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.ViewModel;
using Microsoft.Win32;

namespace ImageEditor.Command
{
    class SaveAsCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public SaveAsCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.Image != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object param)
        {
            EditableImage image = _viewModel.Image;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "." + image.Format.ToString().ToLower(),
                Filter = Properties.Resources.filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                // format ?
                image.Source.Save(saveFileDialog.FileName);
            }
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
