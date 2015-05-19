using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ImageEditor.Annotations;
using ImageEditor.Command;
using ImageEditor.Model;
using ImageEditor.Model.Tool;
using ImageProcessing;
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
                ImageToDisplay = value.Clone();
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CanvasWidth");
            }
        }

        public EditableImage ImageToDisplay
        {
            get { return _tempImage; }
            set { _tempImage = value; OnPropertyChanged();}
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

        private double _horizontalOffset;
        public double HorizontalOffset
        {
            get { return _horizontalOffset; }
            set { _horizontalOffset = value; OnPropertyChanged(); }
        }

        private double _verticalOffset;
        public double VerticalOffset
        {
            get { return _verticalOffset; }
            set { _verticalOffset = value; OnPropertyChanged(); }
        }

        private bool _active;

        public bool Active
        {
            get { return _active; } 
            set { _active = value; OnPropertyChanged(); }
        }

        public Selection Selection;
        public Rectangle SelectedRegion
        {
            get {
                if (Selection != null)
                {
                    return Selection.Active ? Selection.GetRegion() : new Rectangle(0, 0, _image.Width, _image.Height);    
                }
                return new Rectangle(0, 0, _image.Width, _image.Height);  
            }
        }

        private Tool _selectedTool;
        public Tool SelectedTool { 
            get { return _selectedTool; } 
            set { _selectedTool = value; OnPropertyChanged(); } }

        private int _strokeThickness = 1;
        public int StrokeThickness
        {
            get{return _strokeThickness;} 
            set { _strokeThickness = value; OnPropertyChanged(); }
        }

        private Color _selectedColor = Color.Black;
        public Color SelectedColor
        {
            get { return _selectedColor; } 
            set { _selectedColor = value; OnPropertyChanged(); }
        }



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
            if (SaltAndPapper)
            {
                ImageToDisplay.Source = NoiseGenerator.SaltAndPapper(_image.Source, 
                    new Rectangle(0, 0, _image.Width, _image.Height), NoiseCoverage);
            }
            else
            {
                ImageToDisplay.Source = NoiseGenerator.Additive(_image.Source, 
                    new Rectangle(0, 0, _image.Width, _image.Height), NoiseCoverage);
            }
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
            if (Median)
            {
                ImageToDisplay.Source = NoiseReducer.Median(_image.Source, 
                    new Rectangle(0, 0, _image.Width, _image.Height), MedianRadius);
            }
            else
            {
                ImageToDisplay.Source = NoiseReducer.Bilateral(_image.Source,
                    new Rectangle(0, 0, _image.Width, _image.Height), KernelSize, SpatialFactor, ColourFactor);
            }
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
            ImageToDisplay.Source = HistogramEqualazer.Squeeze(_image.Source,
                new Rectangle(0, 0, _image.Width, _image.Height), HistogramLeft, HistogramRight);
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

            HistogramLeft = 0;
            HistogramRight = 255;

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
           
        }

        public void OnFilterChanged()
        {
            var region = SelectedRegion;
            ImageToDisplay.Source = ImageAdjuster.ChangeBrightness(Image.Source, region, GetBrightnessRate());
            ImageToDisplay.Source = ImageAdjuster.ChangeContrast(ImageToDisplay.Source, region,
                GetExponentialRate(_contrast));
            ImageToDisplay.Source = ImageAdjuster.ChangeSaturation(ImageToDisplay.Source, region, GetSaturationRate());
            float redRate = GetExponentialRate(_red);
            float greenRate = GetExponentialRate(_green);
            float blueRate = GetExponentialRate(_blue);
            ImageToDisplay.Source = ImageAdjuster.ChangeColour(ImageToDisplay.Source, region,
                redRate, greenRate, blueRate);
            OnPropertyChanged("ImageToDisplay");
        }

        private float GetExponentialRate(int value)
        {
            return (float)(Math.Pow(Math.E, (double)(256 + value * 2) / 255) / Math.E);
        }



        private float GetSaturationRate()
        {
            int value = _saturation;
            if (value > 0)
            {
                value *= 10;
            }
            return (float)(255 + value) / 255;
        }

        private float GetBrightnessRate()
        {
            return _brightness / 255f;
        }

        public void OpenImage(string path)
        {
            Image = new EditableImage(path);
            ResetFields();
            ResetTools();
            Active = true;
            if (_imageReady != null) 
                _imageReady();
            // clear command list
        }

        public IReversableCommand ApplyCommand { get; private set; }
        public IReversableCommand OpenCommand { get; private set; }
        public IReversableCommand FlipCommand { get; private set; }
        public IReversableCommand HistogramEqualizeCommand { get; private set; }
        public IReversableCommand CropCommand { get; private set; }
        public IReversableCommand HistogramStretchCommand { get; private set; }
        public IReversableCommand InpaintCommand { get; private set; }
        public IReversableCommand ResizeCommand { get; private set; }
        public IReversableCommand RotateCommand { get; private set; }
        public IReversableCommand SaveAsCommand { get; private set; }
        public IReversableCommand SaveCommand { get; private set; }
        public IReversableCommand ZoomCommand { get; private set; }
        public IReversableCommand ResetCommand { get; private set; }
        public IReversableCommand CloseCommand { get; private set; }
        public IReversableCommand SelectToolCommand { get; private set; }
        public IReversableCommand UndoCommand { get; private set; }
        public IReversableCommand RedoCommand { get; private set; }




        public event RaiseCanChange _imageReady;
        public event RaiseCanChange _toolSelected;

        private readonly ToolBox _toolBox;
        public CommandList ComandList;
        private const int MaxCommandListDepth = 10;

        public ImageEditorViewModel()
        {
            InitCommands();
            ComandList = new CommandList(MaxCommandListDepth);
            SetImageReady();
            _toolSelected += SelectToolCommand.RaiseCanExecuteChanged;
            _toolBox = new ToolBox(this);
            Selection = (Selection)_toolBox.GetTool(ToolType.Selection);
            SelectedTool = _toolBox.GetTool(ToolType.Drag);
        }

        private void SetImageReady()
        {
            _imageReady += ApplyCommand.RaiseCanExecuteChanged;
            _imageReady += FlipCommand.RaiseCanExecuteChanged;
            _imageReady += HistogramEqualizeCommand.RaiseCanExecuteChanged;
            _imageReady += HistogramStretchCommand.RaiseCanExecuteChanged;
            _imageReady += CropCommand.RaiseCanExecuteChanged;
            _imageReady += InpaintCommand.RaiseCanExecuteChanged;
            _imageReady += ResizeCommand.RaiseCanExecuteChanged;
            _imageReady += RotateCommand.RaiseCanExecuteChanged;
            _imageReady += SaveAsCommand.RaiseCanExecuteChanged;
            _imageReady += SaveCommand.RaiseCanExecuteChanged;
            _imageReady += ZoomCommand.RaiseCanExecuteChanged;
            _imageReady += ResetCommand.RaiseCanExecuteChanged;
            _imageReady += SelectToolCommand.RaiseCanExecuteChanged;
        }

        private void InitCommands()
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
            UndoCommand = new UndoCommand(ComandList);
            RedoCommand = new RedoCommand(ComandList);
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

        public void MouseDown(Point position)
        {
            _selectedTool.MouseDown(position);
        }

        public void MouseMove(Point position)
        {
            _selectedTool.MouseMove(position);
        }

        public void MouseUp(Point position)
        {
            _selectedTool.MouseUp(position);
        }
    }
}
