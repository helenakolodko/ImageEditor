using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ImageEditor.Annotations;
using ImageEditor.Command;
using ImageEditor.Model;
using ImageEditor.Model.Tool;

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
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CanvasWidth");
                OnPropertyChanged();
            }
        }

        public Bitmap ImageToDisplay
        {
            get
            {
                if (_tempImage != null)
                {
                    return _tempImage.Source;
                }
                return null;
            }
            set { _tempImage.Source = value; OnPropertyChanged(); }
        }

        private double _zoom = 1;
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
                    OnPropertyChanged("CanvasHeight");
                    OnPropertyChanged("CanvasWidth");
                    // Selectedool.Zoom = value
                    OnPropertyChanged();
                }
            }
        }

        private bool _active = false;
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

        public double CanvasWidth
        {
            get 
            {
                if (_tempImage != null)
                {
                    return _tempImage.Width* Zoom;
                }
                return 0;
            } 
        }

        public double CanvasHeight
        {
            get
            {
                if (_tempImage != null)
                {
                    return _tempImage.Height * Zoom;
                }
                return 0;
            } 
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

        public bool SaltAndPapper { get; set; }

        public void OnNoiseCoverageChanged()
        {
//            if (SaltAndPapper)
//            {
//                _tempImage = NoiseGenerator.SaltAndPapper(_image, new Rectangle(0, 0, _image.Width, _image.Height),
//                    NoiseCoverage);
//            }
//            else
//            {
//                _tempImage = NoiseGenerator.Additive(_image, new Rectangle(0, 0, _image.Width, _image.Height),
//                    NoiseCoverage);
//            }
//
//            ShowImage(_tempImage);
        }

        public bool Median { get; set; }

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

        public void OnNoiseReduceParamsChanged()
        {
//            if (Median)
//            {
//                _tempImage = NoiseReducer.Median(_image, new Rectangle(0, 0, _image.Width, _image.Height),
//                    MedianRadius);
//            }
//            else
//            {
//                _tempImage = NoiseReducer.Bilateral(_image, new Rectangle(0, 0, _image.Width, _image.Height),
//                    KernelSize, SpatialFactor, ColourFactor);
//            }
//            ShowImage(_tempImage);
        }

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

        public void OnHistogramBoundsChanged()
        {
//            _tempImage = HistogramEqualazer.Squeeze(_image, new Rectangle(0, 0, _image.Width, _image.Height),
//                HistogramLeft, HistogramRight);
//            ShowImage(_tempImage);
//            Histogram.Image = _tempImage;
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
            // reset all tools
            Zoom = 1;
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
        public void OpenImage(string path)
        {
            Image = new EditableImage(path);
            ResetFields();
            ResetTools();
            Active = true;
        }


        public ICommand ApplyCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }        
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
        public ICommand SelectToolCommand { get; private set; }


        private readonly ToolBox _toolBox;

        public ImageEditorViewModel()
        {
            OpenCommand = new OpenCommand(this);
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
            SelectToolCommand = new SelectToolCommand(this);
            _toolBox = new ToolBox(this);
            _selection = (Selection)_toolBox.GetTool(ToolType.Selection);
            SelectedTool = _toolBox.GetTool(ToolType.Drag);
        }

        public void GetTool(ToolType type)
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
