using System;
using System.Drawing;
using ImageEditor.ViewModel;
using Point = System.Windows.Point;

namespace ImageEditor.Model.Tool
{
    class Bucket : EndPointTool
    {
        public Bucket(ImageEditorViewModel viewModel) : base(viewModel)
        {
        }

        protected override void ProcessPoint(Point position)
        {
            if (ViewModel.Selection.Active)
            {
                Rectangle selection = ViewModel.Selection.GetRegion();
                if (position.X > selection.Left && position.X < selection.Right &&
                        position.Y < selection.Bottom && position.Y > selection.Top)
                {
                    ViewModel.ComandList.AddNew(new Bitmap(ViewModel.Image.Source));
                    ViewModel.Image.FillRegion(selection, ViewModel.SelectedColor);
                    ViewModel.RefreshImage();
                    ViewModel.OnCommandExecuted();
                }   
            }
        }
    }
}
