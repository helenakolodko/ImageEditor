using System;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class SaveCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public SaveCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param)
        {
            EditableImage image = _viewModel.Image;
            image.Source.Save(image.Path);
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
