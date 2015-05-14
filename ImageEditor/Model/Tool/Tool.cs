using System.Windows;

namespace ImageEditor.Model.Tool
{
    abstract class Tool
    {
        private double _zoom;
        public double Zoom { get; set; }

        private Point _startPoint;

        public abstract void MouseDown(Point position);
        public abstract void MouseMove(Point position);
        public abstract void MouseUp(Point position);

    }
}
