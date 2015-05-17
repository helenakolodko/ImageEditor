using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ImageEditor.Model.Tool
{
    class Selection : SelectTool
    {
        private readonly Rectangle _shape;
        private System.Drawing.Rectangle _region;
        public double Zoom
        {
            private get { return _zoom; }
            set
            {
                _zoom = value;
                _startPoint.X = _region.X * value;
                _startPoint.Y = _region.Y * value;
                TopLeft = _startPoint;
                Width = _region.Width * value;
                Height = _region.Height * value;
            }
            
        }
        public bool Active {
            get { return _active; } 
            set
            {
                _shape.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            } 
        }

        public Selection(Rectangle shape)
        {
            _shape = shape;
            Active = false;
            _region = new System.Drawing.Rectangle((int)shape.Margin.Left, (int)shape.Margin.Top,
                (int)shape.Width, (int)shape.Height);
        }

        public System.Drawing.Rectangle GetRegion()
        {
            return _region;
        }

        protected Point StartPoint
        {
            set
            {
                _startPoint = value;
                TopLeft = value;
            }
        }

        protected Point EndPoint
        {
            set
            {
                double xDelta = value.X - _startPoint.X;
                double yDelta = value.Y - _startPoint.Y;
                Point newTopLeft = TopLeft;
                if (xDelta < 0)
                {
                    newTopLeft.X = value.X;
                    xDelta = -xDelta;
                }
                else
                {
                    newTopLeft.X = _startPoint.X;
                }

                if (yDelta < 0)
                {
                    newTopLeft.Y = value.Y;
                    yDelta = -yDelta;
                }
                else
                {
                    newTopLeft.Y = _startPoint.Y;
                }

                Width = xDelta;
                Height = yDelta;
                TopLeft = newTopLeft;
            }
        }

        private Point TopLeft
        {
            get
            {
                return new Point(_shape.Margin.Left, _shape.Margin.Top);
            }
            set
            {
                Thickness margin = _shape.Margin;
                margin.Top = value.Y;
                margin.Left = value.X;
                _shape.Margin = margin;
                _region.Y = (int)(value.Y / Zoom);
                _region.X = (int)(value.X / Zoom);
            } 
        }

        private double Width 
        {
            set
            {
                _shape.Width = value;
                _region.Width = (int)(value / Zoom);
            }
        }
        private double Height
        {
            set
            {
                _shape.Height = value;
                _region.Height = (int)(value / Zoom);
            }
        }
    }
}
