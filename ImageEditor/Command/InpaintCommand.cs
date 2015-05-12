using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class InpaintCommand :ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public InpaintCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Execute(object param)
        {
            throw new System.NotImplementedException();
        }

        public void Undo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}