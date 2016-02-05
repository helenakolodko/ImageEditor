using System;
using System.Drawing;
using System.Windows.Input;
using ImageEditor.ViewModel;
using ImageProcessing;

namespace ImageEditor.Command
{
    public class InpaintCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel viewModel;

        public InpaintCommand(ImageEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.Image != null;
        }

        public void Execute(object param)
        {
            viewModel.ComandList.AddNew(viewModel.Image.Source);
            if (viewModel.Inpainting.CreateMask)
            {
                ColourInpaint.Inpaint(viewModel.Image.Source, ColourInpaint.ObtainMaskMatrix(viewModel.Mask),
                    viewModel.Inpainting.LbpWindowSize, viewModel.Inpainting.InpaintBlockSize);
            }
            else
            {
                ColourInpaint.Inpaint(viewModel.Image.Source, viewModel.SelectedColor,
                    viewModel.Inpainting.LbpWindowSize, viewModel.Inpainting.InpaintBlockSize);    
            }
            viewModel.RefreshImage();
            viewModel.OnCommandExecuted();
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