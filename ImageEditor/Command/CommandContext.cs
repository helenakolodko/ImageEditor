namespace ImageEditor.Command
{
    public class CommandContext
    {
        private ICommand _command;
        // imageEditor context
        // command params
        public CommandContext(ICommand command)
        {
            _command = command;
        }
    }
}