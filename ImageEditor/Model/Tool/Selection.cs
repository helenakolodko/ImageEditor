using System.Drawing;
using System.Windows.Input;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;

namespace ImageEditor.Model.Tool
{
    public class Selection : FixedPointsTool
    {
        public override void RaiseOnZoomChanged()
        {
            throw new System.NotImplementedException();
        }

        private bool _active;
        public bool Active {
            get { return _active; } 
            set { _active = value;} 
        }

        private Rectangle _region;

        public System.Drawing.Rectangle GetRegion()
        {
            return _region;
        }

        public Selection(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void SetStartPoint(Point position)
        {
            throw new System.NotImplementedException();
        }

        public override Cursor GetCursor()
        {
            return Cursors.Cross;
        }

        protected override void SetEndPoint(Point position)
        {
            throw new System.NotImplementedException();
        }
    }
}
