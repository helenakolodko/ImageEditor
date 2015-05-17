using System.Windows;

namespace ImageEditor.Model.Tool
{
    public abstract class DrawingTool : Tool
    {
        public override void MouseDown(Point position)
        {
            SetStartPoint(position);
            SetEndPoint(position);
            _draw = true;
        }

        public override void MouseMove(Point position)
        {
            if (_draw)
            {
                SetEndPoint(position);
            }
        }

        public override void MouseUp(Point position)
        {
            if (_draw)
            {
                Finish(position);
                _draw = false;
            }
        }

        protected abstract void Finish(Point value);

        protected abstract void SetStartPoint(Point value);
        
        protected abstract void SetEndPoint(Point value);
    }
}
