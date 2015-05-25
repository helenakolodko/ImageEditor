using System;
using System.Drawing;
using System.Windows.Input;
using ImageEditor.ViewModel;
using ImageProcessing;

namespace ImageEditor.Command
{
    public class InpaintCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public InpaintCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public void Execute(object param)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            if (_viewModel.CreateMask)
            {
                ColourInpaint.Inpaint(_viewModel.Image.Source, ColourInpaint.ObtainMaskMatrix(_viewModel.Mask),
                    _viewModel.LbpWindowSize, _viewModel.InpaintBlockSize);
            }
            else
            {
                ColourInpaint.Inpaint(_viewModel.Image.Source, _viewModel.SelectedColor,
                    _viewModel.LbpWindowSize, _viewModel.InpaintBlockSize);    
            }
            _viewModel.RefreshImage();
            _viewModel.OnCommandExecuted();
        }

        public event EventHandler CanExecuteChanged;

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
    }
}