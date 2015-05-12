using System;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class CropCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public CropCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Execute(object param)
        {
            throw new NotImplementedException();
        }

        public void Undo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}
