using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class HistogramEqualizeCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public HistogramEqualizeCommand(ImageEditorViewModel viewModel)
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