using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.Properties;
using ImageEditor.ViewModel;
using Microsoft.Win32;

namespace ImageEditor.Command
{
    class SaveAsCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public SaveAsCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object param)
        {
            EditableImage image = _viewModel.Image;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "." + image.Format.ToString().ToLower(),
                Filter = Resources.filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                image.Source.Save(saveFileDialog.FileName, GetImageFormat(saveFileDialog.FileName));
            }
        }

        private ImageFormat GetImageFormat(string imagePath)
        {
            ImageFormat result = ImageFormat.Png;
            string extention = Path.GetExtension(imagePath);
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

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
