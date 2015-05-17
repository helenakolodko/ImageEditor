using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Math.Random;
using ImageEditor.Annotations;
using ImageEditor.Model.Tool;
using ImageProcessing;
using Image = System.Drawing.Image;
using Pen = ImageEditor.Model.Tool.Pen;
using Point = System.Windows.Point;

namespace ImageEditor.View
{
    // selectedColor
    // undo
    // redo  
    internal enum Tool {Hand, Select, Eyedropper, Pen, Brush, Line, Bucket, Crop};

    public partial class MainView : INotifyPropertyChanged
    {
        private ImageEditor.Model.Tool.Tool _line;
        public MainView()
        {
            InitializeComponent();
            SelectedTool = Tool.Hand;
            Zoom = 1.0;
            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            _line = new ImageEditor.Model.Tool.Brush(ImageEdit, ColorPicker, DrawingGrid);
        }

        private bool _active;
        public bool Active { get { return _active; } set { _active = value; OnPropertyChanged(); } }

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

        #region Noise

        public bool SaltAndPapper { get; set; }
        public bool Median { get; set; }

        private int _noiseAmount;

        public int NoiseAmount
        {
            get { return _noiseAmount; }
            set
            {
                _noiseAmount = Math.Max(0, Math.Min(100, value));
                OnPropertyChanged();
            }
        }

        public void OnNoiseCoverageChanged()
        {
            if (SaltAndPapper)
            {
                _tempImage = NoiseGenerator.SaltAndPapper(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                    NoiseAmount);
            }
            else
            {
                _tempImage = NoiseGenerator.Additive(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                    NoiseAmount);
            }

            ShowImage(_tempImage);
        }

        private int _radius;

        public int ReductionRadius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged();
            }
        }

        private int _kernelSize = 3;
        public int KernelSize
        {
            get { return _kernelSize; }
            set
            {
                _kernelSize = value;
                OnPropertyChanged();
            }
        }

        private int _spacialFactor;

        public int SpatialFactor
        {
            get { return _spacialFactor; }
            set
            {
                _spacialFactor = value;
                OnPropertyChanged();
            }
        }

        private int _colourFactor;

        public int ColourFactor
        {
            get { return _colourFactor; }
            set
            {
                _colourFactor = value;
                OnPropertyChanged();
            }
        }

        public void OnNoiseReduceParamsChanged()
        {
            if (Median)
            {
                _tempImage = NoiseReducer.Median(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                    ReductionRadius);
            }
            else
            {
                _tempImage = NoiseReducer.Bilateral(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                    KernelSize, SpatialFactor, ColourFactor);
            }
            ShowImage(_tempImage);
        }

        #endregion


        private Image _image;
        private Image _tempImage;
        private double _zoom;
        private Tool _selectedTool;
        private Point _startPoint;
        private bool _draw;


        private Tool SelectedTool
        {
            get { return _selectedTool; }
            set 
            { 
                _selectedTool = value;
                switch (value)
                {
                    case Tool.Hand:
                        ImageEdit.Cursor = Cursors.Hand;
                        break;
                    case Tool.Select:
                        ImageEdit.Cursor = Cursors.Cross;
                        break;
                    case Tool.Eyedropper:
                        ImageEdit.Cursor = Cursors.IBeam;
                        break;
                    default:
                        ImageEdit.Cursor = Cursors.Arrow;
                        break;
                }
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

        private double _width;
        private double _height;
        public double CanvasWidth { 
            get { return _width; } 
            set { _width = value; OnPropertyChanged(); } }

        public double CanvasHeight
        {
            get { return _height; } 
            set { _height = value; OnPropertyChanged(); }
        }

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

        #region Histogram

        private int _histogramLeft;
        private int _histogramRight = 255;

        public int HistogramLeft
        {
            get { return _histogramLeft; }
            set
            {
                if (_histogramRight >= value)
                {
                    _histogramLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        public int HistogramRight
        {
            get { return _histogramRight; }
            set
            {
                if (_histogramLeft <= value)
                {
                    _histogramRight = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Equalize_OnClick(object sender, RoutedEventArgs e)
        {
            _image = HistogramEqualazer.Equalize(_image, new Rectangle(0, 0, _image.Width, _image.Height));
            ShowImage(_image);
            Histogram.Image = _image;
        }

        private void Stretch_OnClick(object sender, RoutedEventArgs e)
        {
            _image = HistogramEqualazer.Stretch(_image, new Rectangle(0, 0, _image.Width, _image.Height));
            ShowImage(_image);
            Histogram.Image = _image;
        }

        public void OnHistogramBoundsChanged()
        {
            _tempImage = HistogramEqualazer.Squeeze(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                HistogramLeft, HistogramRight);
            ShowImage(_tempImage);
            Histogram.Image = _tempImage;
        }

        #endregion


        public int LbpWindowSize { get; set; }
        public int InpaintBlockSize { get; set; }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = ImageEditor.Properties.Resources.filter;
            if (openFileDialog.ShowDialog() == true)
            {
                OpenImage(openFileDialog.FileName);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom += .25f;
            ZoomOut.IsEnabled = true;
        }

        private void ZoomImage()
        {
            if (_image != null)
            {
                CanvasWidth = _zoom * _image.Width;
                CanvasHeight = _zoom * _image.Height;
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
             Zoom -= _zoom > .25f ? .25f : .1f;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = ImageEditor.Properties.Resources.filter;
            if (saveFileDialog.ShowDialog() == true)
            {
                
            }
        }

        private void OpenImage(string imagePath)
        {
            _image = new Bitmap(imagePath);
            if (_image.PixelFormat != PixelFormat.Format32bppArgb)
            {
                Bitmap b = (Bitmap) _image;
                _image = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
            }
            ImageScroller.Visibility = Visibility.Visible;
            Zoom = 1.0;
            Active = true;
            ShowImage(_image);
            Histogram.Image = _image;
        }

        private void ShowImage(System.Drawing.Image image)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            ImageEdit.Source = bi;
        }

        private void ImageEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _draw = true;
            _startPoint = e.GetPosition(CanvasBorder);
            if (SelectedTool == Tool.Select)
            {
                _line.MouseDown(_startPoint);   
            }
            if (SelectedTool == Tool.Eyedropper)
            {
                GetColour(_startPoint);
            }
        }

        private void GetColour(Point point)
        {
            var b = (Bitmap) _image;
            Color c = b.GetPixel((int) point.X, (int) point.Y);
            ColorPicker.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private void ImageEdit_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draw)
            {
                switch (_selectedTool)
                {
                    case Tool.Hand:
                        MoveImage(e.GetPosition(CanvasBorder));
                        break;
                    case Tool.Select:
                        _line.MouseMove(e.GetPosition(CanvasBorder));
                        break;
                    case Tool.Eyedropper:
                        GetColour(e.GetPosition(CanvasBorder));
                        break;
                }
            }
        }

        private void DrawSelectionRectangle(Point point)
        {
            Selection.Height = point.Y - _startPoint.Y;
            Selection.Width = point.X - _startPoint.X;
        }

        private void MoveImage(Point point)
        {
            ImageScroller.ScrollToHorizontalOffset(ImageScroller.HorizontalOffset + _startPoint.X - point.X);
            ImageScroller.ScrollToVerticalOffset(ImageScroller.VerticalOffset + _startPoint.Y - point.Y);
            ImageScroller.UpdateLayout();
        }

        private void ImageEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draw)
            {
                switch (_selectedTool)
                {
                    case Tool.Hand:
                        MoveImage(e.GetPosition(CanvasBorder));
                        break;
                    case Tool.Select:
                        _line.MouseUp(e.GetPosition(CanvasBorder));
                        break;
                    case Tool.Eyedropper:
                        GetColour(e.GetPosition(CanvasBorder));
                        break;
                }
            }
            _draw = false;
        }

        private void Drag_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tool.Hand;
        }

        private void SelectRegion_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tool.Select;
        }

        private void DropImage(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                OpenImage(files[0]);
            }
        }

        private void DragImageOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = false;
        }


        private void Inpaint_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color c = ColorPicker.SelectedColor;
            Color c1 = Color.FromArgb(c.R, c.G, c.B);
           // _tempImage = ColourInpaint.DoInpaint((Bitmap) _image, 15, c1);
            ShowImage(_tempImage);
        }

        private void SelectColour_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tool.Eyedropper;
        }

        #region NotifyProprrtyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
