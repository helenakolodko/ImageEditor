using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageEditor.Annotations;
using ImageEditor.Model;
using ImageEditor.Model.Tool;
using ImageProcessing;
using Microsoft.Win32;
using Point = System.Windows.Point;

namespace ImageEditor.ViewModel
{
    public class ImageEditorViewModel : INotifyPropertyChanged
    {
        private int _brightness;
        public int Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                OnPropertyChanged();
            }
        }

        private int _contrast;
        public int Contrast
        {
            get { return _contrast; }
            set
            {
                _contrast = value;
                OnPropertyChanged();
            }
        }

        private int _saturation;
        public int Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                OnPropertyChanged();
            }
        }


        private int _red;
        public int Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged();
            }
        }

        private int _green;
        public int Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged();
            }
        }

        private int _blue;
        public int Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged();
            }
        }

        private EditableImage _image;
        private EditableImage _tempImage;

        public EditableImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                _tempImage = value;
            }
        }

        public float NoiseCoverage { get; set; }

        public int ReductionRadius { get; set; }
        public int SpatialFactor { get; set; }
        public int ColourFactor { get; set; }



        public float Zoom { get; set; }

        public Color SelectedColor { get; set; }



        //
//        public MainWindow()
//        {
//            InitializeComponent();
//            _selection = new Selection(Selection);
//            SelectedTool = Tool.Hand;
//            ImageScroller.Visibility = Visibility.Hidden;
//            _zoom = 1.0f;
//        }
//
//        private Image _image;
//        private Image _tempImage;
//        private float _zoom;
//        private Selection _selection;
//        private ToolType _selectedTool;
//        private Point _startPoint;
//        private bool _draw;
//        private string _imagePath;
//        private ImageFormat _imageFormat;
//        private ToolType SelectedTool
//        {
//            get { return _selectedTool; }
//            set
//            {
//                _selectedTool = value;
//                _tempImage = _image;
//                switch (value)
//                {
//                    case ToolType.Drag:
//                        ImageEdit.Cursor = Cursors.Hand;
//                        _selection.Active = false;
//                        _selection.StartPoint = new Point(0, 0);
//                        _selection.EndPoint = new Point(CanvasBorder.Width, CanvasBorder.Height);
//                        break;
//                    case ToolType.SelectRect:
//                        _selection.Active = true;
//                        ImageEdit.Cursor = Cursors.Cross;
//                        break;
//                    default:
//                        ImageEdit.Cursor = Cursors.Arrow;
//                        break;
//                }
//            }
//        }
//        private void Open_Click(object sender, RoutedEventArgs e)
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = Properties.Resources.filter};
//            if (openFileDialog.ShowDialog() == true)
//            {
//                OpenImage(openFileDialog.FileName);
//            }
//        }
//
//        private void OpenImage(string imagePath)
//        {
//            _imagePath = imagePath;
//            _imageFormat = GetImageFormat(imagePath);
//            _image = new Bitmap(imagePath);
//            _tempImage = _image;
//            ImageScroller.Visibility = Visibility.Visible;
//            _zoom = 1;
//            _selection.StartPoint = new Point(0, 0);
//            ZoomImage();
//            _selection.EndPoint = new Point(CanvasBorder.Width, CanvasBorder.Height);
//            Reset();
//        }
//
//        private void ShowImage(Image image)
//        {
//            MemoryStream ms = new MemoryStream();
//            image.Save(ms, ImageFormat.Jpeg);
//            ms.Position = 0;
//            BitmapImage bi = new BitmapImage();
//            bi.BeginInit();
//            bi.StreamSource = ms;
//            bi.EndInit();
//            ImageEdit.Source = bi;
//        }
//
//        private void Save_Click(object sender, RoutedEventArgs e)
//        {
//            if (_imagePath != "")
//            {
//                _image.Save(_imagePath);
//            }
//        }
//
        public void ResetFields()
        {
            // reset all adjustments, show initial image
        }

        public void ResetTools()
        {
            // reset all tools, zoom, show imageScroller
        }
//
//        private void SaveAs_Click(object sender, RoutedEventArgs e)
//        {
//            SaveFileDialog saveFileDialog = new SaveFileDialog
//            {
//                DefaultExt = "." + _imageFormat.ToString().ToLower(),
//                Filter = Properties.Resources.filter
//            };
//            if (saveFileDialog.ShowDialog() == true)
//            {
//                _image.Save(saveFileDialog.FileName, _imageFormat);
//            }
//        }
//
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
//        private void ZoomImage()
//        {
//            ImageScroller.ScrollToHorizontalOffset(0);
//            ImageScroller.ScrollToVerticalOffset(0);
//            if (_image != null)
//            {
//                CanvasBorder.Width = _zoom * _image.Width;
//                CanvasBorder.Height = _zoom * _image.Height;
//                _selection.Zoom = _zoom;
//            }
//        }
//        
//
//        public void OnAdjustmentChanged()
//        {
//            var region = GetSelectionRect();
//            _tempImage = _image;
//            _tempImage = ImageAdjuster.ChangeBrightness(_image, region, Brightness / 255f);
//            _tempImage = ImageAdjuster.ChangeContrast(_tempImage, region, GetRate(Contrast));
//            int value = Saturation;
//            if (value > 0) { value *= 10; }
//            var rate = (float)(255 + value) / 255;
//            _tempImage = ImageAdjuster.ChangeSaturation(_tempImage, region, rate);
//            _tempImage = ImageAdjuster.ChangeColour(_tempImage, region, GetRate(Red), GetRate(Green), GetRate(Blue));
//            ShowImage(_tempImage);
//        }
//
//        private float GetRate(int value)
//        {
//            return (float)(Math.Pow(Math.E, (double)(256 + value * 2) / 255) / Math.E);
//        }
//
//
//        private Rectangle GetSelectionRect()
//        {
//            return _selection.GetRegion();
//        }
//
//        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
//        {
//            _image = _tempImage;
//            Reset();
//        }
//
//        private void ImageEdit_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            _draw = true;
//            _startPoint = e.GetPosition(CanvasBorder);
//            if (SelectedTool == Tool.Select)
//            {
//                _selection.StartPoint = _startPoint;
//                _selection.EndPoint = _startPoint;
//            }
//        }
//
//        private void ImageEdit_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (_draw)
//            {
//                switch (_selectedTool)
//                {
//                    case Tool.Hand:
//                        MoveImage(e.GetPosition(CanvasBorder));
//                        break;
//                    case Tool.Select:
//                        _selection.EndPoint = e.GetPosition(CanvasBorder);
//                        break;
//                }
//            }
//        }
//
//        private void MoveImage(Point point)
//        {
//            ImageScroller.ScrollToHorizontalOffset(ImageScroller.HorizontalOffset + _startPoint.X - point.X);
//            ImageScroller.ScrollToVerticalOffset(ImageScroller.VerticalOffset + _startPoint.Y - point.Y);
//            ImageScroller.UpdateLayout();
//        }
//
//        private void ImageEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            if (_draw)
//            {
//                switch (_selectedTool)
//                {
//                    case Tool.Hand:
//                        MoveImage(e.GetPosition(CanvasBorder));
//                        break;
//                    case Tool.Select:
//                        _selection.EndPoint = e.GetPosition(CanvasBorder);
//                        break;
//                }
//            }
//            _draw = false;
//        }
//
//        private void Drag_Click(object sender, RoutedEventArgs e)
//        {
//            SelectedTool = ToolType.Drag;
//        }
//
//        private void SelectRegion_Click(object sender, RoutedEventArgs e)
//        {
//            SelectedTool = Tool.Select;
//        }
//
//        private void ImageDrop(object sender, DragEventArgs e)
//        {
//            if (e.Data.GetDataPresent(DataFormats.FileDrop))
//            {
//                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
//                OpenImage(files[0]);
//            }
//        }
//
//        private void ImageDragOver(object sender, DragEventArgs e)
//        {
//            e.Effects = e.Data.GetDataPresent(DataFormats.Bitmap) ? DragDropEffects.All : DragDropEffects.None;
//            e.Handled = false;
//        }
//
//        private void Cancel_OnClick(object sender, RoutedEventArgs e)
//        {
//            Reset();
//        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
