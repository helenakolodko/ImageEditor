using ImageEditor.ViewModel;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageEditor.Command
{
    public class PrewittCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;
        public PrewittCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public void Execute(object parameter)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            _viewModel.Image.Source = PrewittOperator.Process(_viewModel.Image.Source, _viewModel.SelectedRegion);
            _viewModel.RefreshImage();
            _viewModel.OnCommandExecuted();
        }

        public void Undo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
