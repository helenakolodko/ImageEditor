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
using ImageEditor.Command;
using ImageEditor.Model;
using ImageEditor.Model.Tool;
using ImageProcessing;
using Microsoft.Win32;
using Point = System.Windows.Point;

namespace ImageEditor.ViewModel
{
    public class ImageEditorViewModel : INotifyPropertyChanged
    {
        private EditableImage _image;
        private EditableImage _tempImage;

        public EditableImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                _tempImage = value;
                ImageToDisplay = value.Source;
            }
        }

        public Bitmap ImageToDisplay
        {
            get { return _tempImage.Source; }
            set { _tempImage.Source = value; OnPropertyChanged(); }
        }

        private double _zoom;
        public double Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if (value > 0e-3)
                {
                    _zoom = value;
                    ZoomImage();
                    // Selectedool.Zoom = value
                    OnPropertyChanged();
                }
            }
        }

        private bool _active;
        public bool Active { get { return _active; } set { _active = value; OnPropertyChanged(); } }

        private Selection _selection;
        public Rectangle SelectedRegion
        {
            get {
                return _selection.Active ? _selection.GetRegion() : new Rectangle(0, 0, _image.Width, _image.Height);
            }
        }

        public Tool SelectedTool { get; set; }

        public Color SelectedColor { get; set; }

        private double _width;

        public double CanvasWidth
        {
            get { return _width; } 
            set { _width = value; OnPropertyChanged(); }
        }

        private double _height;

        public double CanvasHeight
        {
            get { return _height; } 
            set { _height = value; OnPropertyChanged(); }
        }

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

        private int _imageWidth;
        public int ImageWidth
        {
            get { return _imageWidth; } 
            set { _imageWidth = value; OnPropertyChanged(); }
        }

        private int _imageHeight;
        public int ImageHeight
        {
            get { return _imageHeight; }
            set { _imageHeight = value; OnPropertyChanged(); }
        }

        private int _noiseCoverage;

        public int NoiseCoverage
        {
            get { return _noiseCoverage; } 
            set { _noiseCoverage = value; OnPropertyChanged(); }
        }

        private int _medianRadius;
        public int MedianRadius
        {
            get { return _medianRadius; }
            set { _medianRadius = value; OnPropertyChanged(); }
        }

        private int _kernelSize;
        public int KernelSize
        {
            get { return _kernelSize; }
            set { _kernelSize = value; OnPropertyChanged(); }
        }

        private int _spatialFactor;
        public int SpatialFactor
        {
            get { return _spatialFactor; }
            set { _spatialFactor = value; OnPropertyChanged(); }
        }

        private int _colourFactor;
        public int ColourFactor
        {
            get { return _colourFactor; }
            set { _colourFactor = value; OnPropertyChanged(); }
        }

        private int _lbpWindowSize;
        public int LbpWindowSize
        {
            get { return _lbpWindowSize; }
            set { _lbpWindowSize = value; OnPropertyChanged(); }
        }

        private int _inpaintBlockSize;
        public int InpaintBlockSize
        {
            get { return _inpaintBlockSize; }
            set { _inpaintBlockSize = value; OnPropertyChanged(); }
        }
        

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
//                    case ToolType.Selection:
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
            Brightness = 0;
            Contrast = 0;
            Saturation = 0;
            Red = 0;
            Green = 0;
            Blue = 0;
            Brightness = 0;
            NoiseCoverage = 0;
            MedianRadius = 1;
            KernelSize = 3;
            SpatialFactor = 0;
            ColourFactor = 0;
            LbpWindowSize = 0;
            InpaintBlockSize = 0;
            // show initial image
        }

        public void ApplyChanges()
        {
            // ???
            Image.Source = _tempImage.Source;
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
        private void ZoomImage()
        {
            if (_image != null)
            {
                CanvasWidth = _zoom * _image.Width;
                CanvasHeight = _zoom * _image.Height;
            }
        }
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

        public ICommand ApplyCommand { get; private set; }
        public ICommand FlipCommand { get; private set; }
        public ICommand HistogramEqualizeCommand { get; private set; }
        public ICommand CropCommand { get; private set; }
        public ICommand HistogramStretchCommand { get; private set; }
        public ICommand InpaintCommand { get; private set; }
        public ICommand ResizeCommand { get; private set; }
        public ICommand RotateCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ZoomCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        private readonly ToolBox _toolBox;

        public ImageEditorViewModel()
        {
            ApplyCommand = new ApplyCommand(this);
            FlipCommand = new FlipCommand(this);
            HistogramEqualizeCommand = new HistogramEqualizeCommand(this);
            HistogramStretchCommand = new HistogramStretchCommand(this);
            CropCommand = new CropCommand(this);
            InpaintCommand = new InpaintCommand(this);
            ResizeCommand = new ResizeCommand(this);
            RotateCommand = new RotateCommand(this);
            SaveAsCommand = new SaveAsCommand(this);
            SaveCommand = new SaveCommand(this);
            ZoomCommand = new ZoomCommand(this);
            ResetCommand = new ResetCommand(this);
            CloseCommand = new CloseCommand(this);
            _toolBox = new ToolBox(this);
            _selection = (Selection)_toolBox.GetTool(ToolType.Selection);
            SelectedTool = _toolBox.GetTool(ToolType.Drag);
        }

        public Tool GetTool(ToolType type)
        {
            SelectedTool = _toolBox.GetTool(type);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
