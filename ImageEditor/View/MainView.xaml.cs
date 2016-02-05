using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.View
{
    public partial class MainView  
    {
        private ImageEditorViewModel viewModel;
        public MainView()
        {
            InitializeComponent();
            viewModel = new ImageEditorViewModel();
            this.DataContext = viewModel;
        }

        private void ImageEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.MouseDown(e.GetPosition(CanvasBorder));
        }

        private void ImageEdit_MouseMove(object sender, MouseEventArgs e)
        {
            viewModel.MouseMove(e.GetPosition(CanvasBorder));
        }

        private void ImageEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewModel.MouseUp(e.GetPosition(CanvasBorder));
        }

        private void DropImage(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                viewModel.OpenImage(files[0]);
            }
        }

        private void DragImageOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = false;
        }
    }
}
