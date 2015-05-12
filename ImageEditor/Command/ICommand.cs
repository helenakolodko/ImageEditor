namespace ImageEditor.Command
{
    public interface ICommand
    {
        void Execute(object param);
        void Undo(CommandContext context);
        void Redo(CommandContext context);
    }
}
