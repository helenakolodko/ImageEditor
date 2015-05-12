using System.Collections.Generic;

namespace ImageEditor.Command
{
    public class CommandList
    {
        private Queue<CommandContext> _toUndo;
        private Queue<CommandContext> _toRedo;
        private int _maxDepth;

        public CommandList(int maxDepth)
        {
            _maxDepth = maxDepth;
            _toRedo = new Queue<CommandContext>();
            _toUndo = new Queue<CommandContext>();
        }

        public bool Undo()
        {
            return false;
        }

        public bool Redo()
        {
            return false;
        }
    }
}