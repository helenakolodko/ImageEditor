using System.Windows;

namespace ImageEditor.Model.Tool
{
    public abstract class Tool
    {
        protected double _zoom;
        public double Zoom { get; set; }

        protected Point _startPoint;
        protected bool _draw;

        protected bool _active;
        public bool Active{ get; set; }

        public abstract void MouseDown(Point position);
        public abstract void MouseMove(Point position);
        public abstract void MouseUp(Point position);

    }
}
