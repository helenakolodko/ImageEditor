namespace ImageEditor.Command
{
    public class CommandContext
    {
        private IReversableCommand _command;
        // imageEditor context
        // command params
        public CommandContext(IReversableCommand command)
        {
            _command = command;
        }
    }
}