using System.Windows;
using System.Windows.Controls;

namespace ImageEditor.Model.Tool
{
    class Drag : ManageTool
    {
        private ScrollViewer _scrollViewer;

        public Point StartPoint { get { return _startPoint; } set { _startPoint = value; } }

        private Point _endPoint;
        public Point EndPoint { get { return _endPoint; } set { _endPoint = value; MoveImage(value); } }

        private void MoveImage(Point point)
        {
            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + _startPoint.X - point.X);
            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + _startPoint.Y - point.Y);
            _scrollViewer.UpdateLayout();
        }

        public Drag(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
        }
    }
}
