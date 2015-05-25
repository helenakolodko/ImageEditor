using System;
using System.Windows.Input;
using ImageEditor.Model.Tool;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class SelectToolCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public SelectToolCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object parameter)
        {
            string type = (string)parameter;
            ToolType result;
            if (ToolType.TryParse(type, true, out result))
            {
                _viewModel.GetTool(result);
            }
            if (result != ToolType.Bucket)
            {
                _viewModel.ResetTools();
            }
        }

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

        public event EventHandler CanExecuteChanged;
    }
}
