using System;

namespace ImageEditor.Command
{

    public delegate void RaiseCanChange();
    public interface IReversableCommand
    {
        void Execute(object parameter);
        void Undo(CommandContext context);
        void Redo(CommandContext context);
        void RaiseCanExecuteChanged();
    }
}
