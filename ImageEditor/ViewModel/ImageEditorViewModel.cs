using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
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
        private EditableImage image;
        private EditableImage tempImage;
        private double zoom = 1;
        private double horizontalOffset;
        private double verticalOffset;
        private bool active;
        private Tool selectedTool;
        private int strokeThickness = 1;
        private Color selectedColor = Color.Black;
        private int imageWidth;
        private int imageHeight;
        private int histogramLeft;
        private int histogramRight = 255;
        private Filters filters = new Filters(255);
        private Noise noise = new Noise();
        private Inpainting inpainting = new Inpainting();
        private readonly ToolBox toolBox;
        private readonly int maxCommandListDepth = 10;

        public event RaiseCanChange ImageReady;
        public event RaiseCanChange ToolSelected;
        public event RaiseCanChange CommandExecuted;
        public event PropertyChangedEventHandler PropertyChanged;

        public ImageEditorViewModel()
        {
            InitCommands();
            ComandList = new CommandList(maxCommandListDepth);
            AddListenersForImageReady();
            toolBox = new ToolBox(this);
            ToolSelected += CropCommand.RaiseCanExecuteChanged;
            CommandExecuted += RedoCommand.RaiseCanExecuteChanged;
            CommandExecuted += UndoCommand.RaiseCanExecuteChanged;
            Selection = (Selection)toolBox.GetTool(ToolType.Selection);
            SelectedTool = toolBox.GetTool(ToolType.Drag);
        }

        public EditableImage Image
        {
            get { return image; }
            set
            {
                image = value;
                ImageToDisplay = value.Clone();
                OnPropertyChanged();
                ImageWidth = Image.Width;
                ImageHeight = Image.Height;
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CanvasWidth");
            }
        }

        public EditableImage ImageToDisplay
        {
            get {
                return tempImage; }
            set { tempImage = value;
                OnPropertyChanged(); }
        }

        public double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                if (value > 0e-3)
                {
                    zoom = value;
                    OnPropertyChanged("CanvasHeight");
                    OnPropertyChanged("CanvasWidth");
                    Selection.Zoom = value;
                    OnPropertyChanged();
                }
            }
        }

        public double HorizontalOffset
        {
            get { return horizontalOffset; }
            set { horizontalOffset = value; OnPropertyChanged(); }
        }

        public double VerticalOffset
        {
            get { return verticalOffset; }
            set { verticalOffset = value; OnPropertyChanged(); }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; OnPropertyChanged(); }
        }

        public Selection Selection { get; set; }

        public Rectangle SelectedRegion
        {
            get
            {
                if (Selection != null)
                {
                    return Selection.Active ? Selection.GetRegion() : new Rectangle(0, 0, image.Width, image.Height);
                }
                return new Rectangle(0, 0, image.Width, image.Height);
            }
        }

        public Tool SelectedTool
        {
            get { return selectedTool; }
            set { selectedTool = value; OnPropertyChanged(); }
        }

        public int StrokeThickness
        {
            get { return strokeThickness; }
            set { strokeThickness = value; OnPropertyChanged(); }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; OnPropertyChanged(); }
        }

        public double CanvasWidth
        {
            get
            {
                if (tempImage != null)
                {
                    return tempImage.Width * Zoom;
                }
                return 0;
            }
        }
        public double CanvasHeight
        {
            get
            {
                if (tempImage != null)
                {
                    return tempImage.Height * Zoom;
                }
                return 0;
            }
        }

        public Filters Filters
        {
            get {
                return filters; }
            set {
                filters = value; OnPropertyChanged(); }
        }
        public Noise Noise
        {
            get { return noise; }
            set { noise = value; OnPropertyChanged(); }
        }
        public Inpainting Inpainting
        {
            get { return inpainting; }
            set { inpainting = value; OnPropertyChanged(); }
        }

        public Bitmap Mask;

        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; OnPropertyChanged(); }
        }

        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; OnPropertyChanged(); }
        }

        public int HistogramLeft
        {
            get { return histogramLeft; }
            set
            {
                if (histogramRight >= value)
                {
                    histogramLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        public int HistogramRight
        {
            get { return histogramRight; }
            set
            {
                if (histogramLeft <= value)
                {
                    histogramRight = value;
                    OnPropertyChanged();
                }
            }
        }


        #region Commands Properties
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
        #endregion

        public CommandList ComandList { get; set; }


        public void RefreshImage()
        {
            ImageToDisplay.Source = new Bitmap(Image.Source);
            OnPropertyChanged("ImageToDisplay");
            ImageWidth = Image.Width;
            ImageHeight = Image.Height;
            OnPropertyChanged("CanvasHeight");
            OnPropertyChanged("CanvasWidth");
        }


        public void ResetFields()
        {
            Filters = new Filters(255);
            Noise.Reset();
            Inpainting = new Inpainting();

            HistogramLeft = 0;
            HistogramRight = 255;

            // show initial image
        }

        public void ApplyChanges()
        {
            // ???
            Image.Source = tempImage.Source;
        }

        public void ResetTools()
        {
            Selection.Reset();
        }

        public void OpenImage(string path)
        {
            Image = new EditableImage(path);
            ResetFields();
            ResetTools();
            Active = true;
            if (ImageReady != null)
                ImageReady();
            // clear command list
        }

        public void OnBrightnessChanged()
        {
            var region = SelectedRegion;
            ImageToDisplay.Source = ImageAdjuster.ChangeBrightness(Image.Source, region, Filters.BrightnessRate);
            OnPropertyChanged("ImageToDisplay");
        }

        public void OnContrastChanged()
        {
            var region = SelectedRegion;
            ImageToDisplay.Source = ImageAdjuster.ChangeContrast(ImageToDisplay.Source, region, Filters.ContrastRate);
            OnPropertyChanged("ImageToDisplay");
        }

        public void OnSaturationChanged()
        {
            var region = SelectedRegion;
            ImageToDisplay.Source = ImageAdjuster.ChangeSaturation(ImageToDisplay.Source, region, Filters.SaturationRate);
            OnPropertyChanged("ImageToDisplay");
        }

        public void OnColourChanged()
        {
            var region = SelectedRegion;
            ImageToDisplay.Source = ImageAdjuster.ChangeColour(ImageToDisplay.Source, region, Filters.RedRate, Filters.GreenRate, Filters.BlueRate);
            OnPropertyChanged("ImageToDisplay");
        }

        public void OnNoiseCoverageChanged()
        {
            if (Noise.SaltAndPapper)
            {
                ImageToDisplay.Source = NoiseGenerator.SaltAndPapper(image.Source,
                    SelectedRegion, Noise.Coverage);
            }
            else
            {
                ImageToDisplay.Source = NoiseGenerator.Additive(image.Source,
                    SelectedRegion, Noise.Coverage);
            }
        }

        public void OnNoiseReduceParamsChanged()
        {
            if (Noise.Median)
            {
                ImageToDisplay.Source = NoiseReducer.Median(image.Source,
                    SelectedRegion, Noise.MedianRadius);
            }
            else
            {
                ImageToDisplay.Source = NoiseReducer.Bilateral(image.Source,
                    SelectedRegion, Noise.KernelSize, Noise.SpatialFactor, Noise.ColourFactor);
            }
        }

        public void OnHistogramBoundsChanged()
        {
            ImageToDisplay.Source = HistogramEqualazer.Squeeze(image.Source,
                SelectedRegion, HistogramLeft, HistogramRight);
        }

        public void OnCommandExecuted()
        {
            CommandExecuted();
        }

        private void AddListenersForImageReady()
        {
            ImageReady += ApplyCommand.RaiseCanExecuteChanged;
            ImageReady += FlipCommand.RaiseCanExecuteChanged;
            ImageReady += HistogramEqualizeCommand.RaiseCanExecuteChanged;
            ImageReady += HistogramStretchCommand.RaiseCanExecuteChanged;
            ImageReady += CropCommand.RaiseCanExecuteChanged;
            ImageReady += InpaintCommand.RaiseCanExecuteChanged;
            ImageReady += ResizeCommand.RaiseCanExecuteChanged;
            ImageReady += RotateCommand.RaiseCanExecuteChanged;
            ImageReady += SaveAsCommand.RaiseCanExecuteChanged;
            ImageReady += SaveCommand.RaiseCanExecuteChanged;
            ImageReady += ZoomCommand.RaiseCanExecuteChanged;
            ImageReady += ResetCommand.RaiseCanExecuteChanged;
            ImageReady += SelectToolCommand.RaiseCanExecuteChanged;
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
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
        }

        public void GetTool(ToolType type)
        {
            if (type == ToolType.Selection)
            {
                Selection.Active = true;
            }
            else if (type != ToolType.Bucket && type != ToolType.Drag)
            {
                Selection.Active = false;
            }
            OnPropertyChanged("Selection");
            SelectedTool = toolBox.GetTool(type);
            if (ToolSelected != null) ToolSelected();
        }

        public void MouseDown(Point position)
        {
            selectedTool.MouseDown(position);
        }

        public void MouseMove(Point position)
        {
            selectedTool.MouseMove(position);
        }

        public void MouseUp(Point position)
        {
            selectedTool.MouseUp(position);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
