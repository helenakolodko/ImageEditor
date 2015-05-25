using System;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class SaveCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public SaveCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object param)
        {
            // ????????????????????????????????????????????
            EditableImage image = _viewModel.Image;
            image.Source.Save(image.Path, image.Format);
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
