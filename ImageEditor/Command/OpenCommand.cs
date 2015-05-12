using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class OpenCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public OpenCommand(ImageEditorViewModel viewModel)
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
