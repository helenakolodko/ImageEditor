using System;
using System.Drawing.Imaging;
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
                image.Source.Save(saveFileDialog.FileName, GetImageFormat(saveFileDialog.FileName));
            }
        }

        private ImageFormat GetImageFormat(string imagePath)
        {
            ImageFormat result = ImageFormat.Png;
            string extention = System.IO.Path.GetExtension(imagePath);
            if (extention == ".jpg")
            {
                result = ImageFormat.Jpeg;
            }
            else if (extention == ".bmp")
            {
                result = ImageFormat.Bmp;
            }
            else if (extention == ".gif")
            {
                result = ImageFormat.Gif;
            }
            return result;
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
