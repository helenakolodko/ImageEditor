using System.Collections.Generic;
using System.Drawing;

namespace ImageEditor.Command
{
    public class CommandList
    {
        private Stack<Bitmap> _toUndo;
        private Stack<Bitmap> _toRedo;

        public bool HasToRedo { get { return _toRedo.Count > 0; } }
        public bool HasToUndo { get { return _toUndo.Count > 0; } }

        public CommandList(int maxDepth)
        {
            _toRedo = new Stack<Bitmap>(maxDepth);
            _toUndo = new Stack<Bitmap>(maxDepth);
        }

        public Bitmap Undo(Bitmap current)
        {
            if (_toUndo.Count > 0)
            {
                _toRedo.Push(current);
                return _toUndo.Pop();
            }
            return current;
        }

        public Bitmap Redo(Bitmap current)
        {
            if (_toRedo.Count > 0)
            {
                _toUndo.Push(current);
                return _toRedo.Pop();
            }
            return current;
        }

        public void AddNew(Bitmap current)
        {
            _toUndo.Push(current);
            _toRedo.Clear();
        }
    }
}