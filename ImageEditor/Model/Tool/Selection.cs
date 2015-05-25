using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ImageEditor.Annotations;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;

namespace ImageEditor.Model.Tool
{
    public class Selection : DrawingTool, INotifyPropertyChanged
    {
        private double _zoom = 1.0;
        public double Zoom
        {
            set
            {
                _zoom = value;
                StartPoint.X = (int) (_region.X * value);
                StartPoint.Y = (int) (_region.Y * value);
                TopLeft = StartPoint;
                Width = (int) (_region.Width * value);
                Height = (int) (_region.Height * value);
            }

        }

        private bool _active;
        public bool Active {
            get { return _active; }
            set
            {
                _active = value;
                if (value == false)
                {
                    ViewModel.RefreshImage();
                    Reset();
                    OnPropertyChanged();
                }
            } 
        }

        private Rectangle _region;

        public Rectangle GetRegion()
        {
            return _region;
        }

        public Selection(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X ), (int)(value.Y));
            _region = new Rectangle((int)(value.X ), (int)(value.Y), 0, 0);
        }

        protected override void SetEndPoint(Point value)
        {
            FormRectangle((int)(value.X), (int)(value.Y));
        }

        protected override void Finish(Point value)
        {
            SetEndPoint(value);
        }

        public void FormRectangle(int x, int y)
        {
            int xDelta = x - StartPoint.X;
            int yDelta = y - StartPoint.Y;
            DrawingPoint newTopLeft = TopLeft;
            if (xDelta < 0)
            {
                newTopLeft.X = x;
                xDelta = -xDelta;
            }
            else
            {
                newTopLeft.X = StartPoint.X;
            }

            if (yDelta < 0)
            {
                newTopLeft.Y = y;
                yDelta = -yDelta;
            }
            else
            {
                newTopLeft.Y = StartPoint.Y;
            }

            Width = xDelta;
            Height = yDelta;
            TopLeft = newTopLeft;
        }

        private DrawingPoint TopLeft
        {
            get
            {
                return new DrawingPoint((int) ViewModel.SelectionMargin.Left, (int) ViewModel.SelectionMargin.Top);
            }
            set
            {
                Thickness SelectionMargin = ViewModel.SelectionMargin;
                SelectionMargin.Top = value.Y;
                SelectionMargin.Left = value.X;
                ViewModel.SelectionMargin = SelectionMargin;
                _region.Y = (int)(value.Y / _zoom);
                _region.X = (int)(value.X / _zoom);
            }
        }

        private double Width
        {
            set
            {
                ViewModel.SelectionWidth = value;
                _region.Width = (int)(value / _zoom);
            }
        }
        private double Height
        {
            set
            {
                ViewModel.SelectionHeight = value;
                _region.Height = (int)(value / _zoom);
            }
        }

        public override Cursor GetCursor()
        {
            return Cursors.Cross;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reset()
        {
            TopLeft = DrawingPoint.Empty;
            Width = ViewModel.CanvasWidth;
            Height = ViewModel.CanvasWidth;
        }
    }
}
