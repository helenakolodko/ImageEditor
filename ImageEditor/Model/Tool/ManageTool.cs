using System.Windows;

namespace ImageEditor.Model.Tool
{
    abstract class ManageTool : Tool
    {
        protected Point StartPoint { get; set; }
        protected Point EndPoint { get; set; }

        public override void MouseDown(Point position)
        {
            StartPoint = position;
            EndPoint = position;
            _draw = true;
        }

        public override void MouseMove(Point position)
        {
            if (_draw)
            {
                EndPoint = position;
            }
        }

        public override void MouseUp(Point position)
        {
            if (_draw)
            {
                EndPoint = position;
                _draw = false;
            }
        }
    }
}
