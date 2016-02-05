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
        private double zoom = 1.0;
        private bool active;
        private Rectangle region;
        private Thickness selectionMargin;
        private double selectionWidth;
        private double selectionHeight;

        public double Zoom
        {
            set
            {
                zoom = value;
                StartPoint.X = (int) (region.X * value);
                StartPoint.Y = (int) (region.Y * value);
                TopLeft = StartPoint;
                Width = (int) (region.Width * value);
                Height = (int) (region.Height * value);
            }

        }

        public bool Active {
            get { return active; }
            set
            {
                active = value;
                if (value == false)
                {
                    ViewModel.RefreshImage();
                    Reset();
                    OnPropertyChanged();
                }
            } 
        }


        public Thickness SelectionMargin
        {
            get { return selectionMargin; }
            set { selectionMargin = value; OnPropertyChanged(); }
        }

        public double SelectionWidth
        {
            get { return selectionWidth; }
            set { selectionWidth = value; OnPropertyChanged(); }
        }

        public double SelectionHeight
        {
            get { return selectionHeight; }
            set { selectionHeight = value; OnPropertyChanged(); }
        }

        public Rectangle GetRegion()
        {
            return region;
        }

        public Selection(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void SetStartPoint(Point value)
        {
            StartPoint = new DrawingPoint((int)(value.X ), (int)(value.Y));
            region = new Rectangle((int)(value.X ), (int)(value.Y), 0, 0);
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
                return new DrawingPoint((int) SelectionMargin.Left, (int) SelectionMargin.Top);
            }
            set
            {
                Thickness selectionMargin = SelectionMargin;
                selectionMargin.Top = value.Y;
                selectionMargin.Left = value.X;
                SelectionMargin = selectionMargin;
                region.Y = (int)(value.Y / zoom);
                region.X = (int)(value.X / zoom);
            }
        }

        private double Width
        {
            set
            {
                SelectionWidth = value;
                region.Width = (int)(value / zoom);
            }
        }
        private double Height
        {
            set
            {
                SelectionHeight = value;
                region.Height = (int)(value / zoom);
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
