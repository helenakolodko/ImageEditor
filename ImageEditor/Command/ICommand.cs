namespace ImageEditor.Command
{
    public interface IReversableCommand
    {
        void Execute(object param);
        void Undo(CommandContext context);
        void Redo(CommandContext context);
    }
}
