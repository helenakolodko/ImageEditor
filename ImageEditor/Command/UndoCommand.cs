using System;
using System.Windows.Input;

namespace ImageEditor.Command
{
    public class UndoCommand : IReversableCommand, ICommand
    {
        private CommandList _commandList;

        public UndoCommand(CommandList commandList)
        {
            _commandList = commandList;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
        }

        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }

        public void RaiseCanExecuteChanged()
        {
            throw new NotImplementedException();
        }
    }
}