using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageEditor.Model;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    class ZoomCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ZoomCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            if (_viewModel.Image != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object param)
        {
       
//            ImageScroller.ScrollToHorizontalOffset(0);
//            ImageScroller.ScrollToVerticalOffset(0);
//            if (_image != null)
//            {
//                CanvasBorder.Width = _ * _viewModel.Image.Width;
//                CanvasBorder.Height = _zoom * _viewModel.Image.Height;
//                _selection.Zoom = _zoom;
//            }
        }

//        private void ZoomIn_Click(object sender, RoutedEventArgs e)
//        {
//            _zoom += .25f;
//            Zoom.Text = (int)(_zoom * 100) + "%";
//            ZoomOut.IsEnabled = true;
//            ZoomImage();
//        }
//
//        private void ZoomOut_Click(object sender, RoutedEventArgs e)
//        {
//            ImageScroller.ScrollToHorizontalOffset(0);
//            ImageScroller.ScrollToVerticalOffset(0);
//            if (_zoom <= .15f)
//            {
//                ZoomOut.IsEnabled = false;
//            }
//            else
//            {
//                _zoom -= _zoom > .25f ? .25f : .1f;
//            }
//            Zoom.Text = (int)(_zoom * 100) + "%";
//            ZoomImage();
//        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
        }

        public void Redo(CommandContext context)
        {
        }
    }
}
