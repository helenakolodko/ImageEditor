using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Math.Random;
using ImageEditor.Annotations;
using ImageProcessing;
using Image = System.Drawing.Image;
using Point = System.Windows.Point;

namespace ImageEditor.View
{
    // selectedColor
    // undo
    // redo  
    internal enum Tool {Hand, Select, Eyedropper, Pen, Brush, Line, Bucket, Crop};

    public partial class MainView : INotifyPropertyChanged
    {
        private float _noiseAmount = 10f;
        public float NoiseAmount
        {
            get { return _noiseAmount; }
            set
            {
                _noiseAmount = Math.Max(0, Math.Min(100, value));
                GenerateNoise();
                OnPropertyChanged();
            }
        }

        private void GenerateNoise()
        {
            int width = _image.Width;
            int height = _image.Height;

            ///// Salt-n-Pepper

            int noisyPixels = (int)((width * height * _noiseAmount) / 100);
            Color[] values = new Color[2] { Color.Black, Color.White };
            Random rand = new Random();
            Bitmap b = new Bitmap(_image, width, height);
            for (int i = 0; i < noisyPixels; i++)
            {
                int x = 0 + rand.Next(width);
                int y = 0 + rand.Next(height);
                int colorPlane = rand.Next(3);

                b.SetPixel(x, y, values[rand.Next(2)]);
            }
            //// Additive

//            IRandomNumberGenerator generator = new UniformGenerator(new Range(-_noiseAmount, _noiseAmount));
//            for (int y = 0; y < height; y++)
//            {
//                for (int x = 0; x < width; x++)
//                {
//                    Color c = b.GetPixel(x, y);
//                    b.SetPixel(x, y, Color.FromArgb((byte)Math.Max(0, Math.Min(255, c.R + generator.Next())),
//                            (byte)Math.Max(0, Math.Min(255, c.G + generator.Next())),
//                            (byte)Math.Max(0, Math.Min(255, c.B + generator.Next()))));
//                }
//            }
            _tempImage = b;
            ShowImage(_tempImage);
        }

        private Image _image;
        private Image _tempImage;
        private float _zoom;
        private Tool _selectedTool;
        private int _brightness;
        private int _contrast;
        private Point _startPoint;
        private bool _draw;
        public int BrightnessValue 
        { 
            get { return _brightness; }
            set { _brightness = value; AdjustBrightness(); } 
        }

        public int ContrastValue
        {
            get { return _contrast; }
            set { _contrast = value; AdjustContrast(); }
        }

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
                    default:
                        ImageEdit.Cursor = Cursors.Arrow;
                        break;
                }
            }
        }

        public MainView()
        {
            InitializeComponent();
            SelectedTool = Tool.Hand;
            ImageScroller.Visibility = Visibility.Hidden;
            _zoom = 1.0f;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = ImageEditor.Properties.Resources.filter;
            if (openFileDialog.ShowDialog() == true)
            {
                //System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(openFileDialog.FileName, true);
                OpenImage(openFileDialog.FileName);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _zoom += .25f;
            Zoom.Text = (int)(_zoom * 100) + "%";
            ZoomOut.IsEnabled = true;
            ZoomImage();
        }

        private void ZoomImage()
        {
            ImageScroller.ScrollToHorizontalOffset(0);
            ImageScroller.ScrollToVerticalOffset(0);
            if (_image != null)
            {
                CanvasBorder.Width = _zoom * _image.Width;
                CanvasBorder.Height = _zoom * _image.Height;
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ImageScroller.ScrollToHorizontalOffset(0);
            ImageScroller.ScrollToVerticalOffset(0);
            if (_zoom <= .15f)
            {
                ZoomOut.IsEnabled = false;
            }
            else
            {
                _zoom -= _zoom > .25f ? .25f : .1f;
            }
            Zoom.Text = (int)(_zoom * 100) + "%";
            ZoomImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            //saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = ImageEditor.Properties.Resources.filter;
            if (saveFileDialog.ShowDialog() == true)
            {
                
            }
        }

        private void OpenImage(string imagePath)
        {
            _image = new System.Drawing.Bitmap(imagePath);
            ImageScroller.Visibility = Visibility.Visible;
            _zoom = 1;
            ZoomImage();
            ShowImage(_image);
            histogram.Image = _image;
        }

        private void ShowImage(System.Drawing.Image image)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            ImageEdit.Source = bi;
        }

        /**
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *      this.WindowState = System.Windows.WindowState.Minimized;
         *      this.nIcon.Icon = new Icon(@"../../Cartman-General.ico");
         *      this.nIcon.ShowBalloonTip(5000, "Hi", "This is a BallonTip from Windows Notification", ToolTipIcon.Info);
         * }
         */

        private void AdjustBrightness()
        {
            float rate = BrightnessValue / 100.0f;

            _tempImage = ImageAdjuster.ChangeBrightness(_image, rate);

            ShowImage(_tempImage);
        }

        private void AdjustContrast()
        {
            float rate = (ContrastValue < 0) ? (ContrastValue + 50.0f) / 50.0f : (ContrastValue / 30.0f + 1);

            _tempImage = ImageAdjuster.ChangeContrast(_image, rate);

            ShowImage(_tempImage);
        }

        private void ApplyBrightness_Click(object sender, RoutedEventArgs e)
        {
            _image = _tempImage;
            _tempImage = null;
            Brightness.Value = 0;
            Contrast.Value = 0;
        }

        private void ImageEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _draw = true;
            _startPoint = e.GetPosition(CanvasBorder);
            if (SelectedTool == Tool.Select)
            {
                Thickness margin = Selection.Margin;
                margin.Left = _startPoint.X;
                margin.Top = _startPoint.Y;
                Selection.Margin = margin;
            }
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
                        DrawSelectionRectangle(e.GetPosition(CanvasBorder));
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
                        DrawSelectionRectangle(e.GetPosition(CanvasBorder));
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

//        public Image GetImage()
//        {
//            if (_image != null)
//            {
//                return _image;
//            }
//            else
//            {
//                return new Bitmap(0, 0);
//            }
//        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Equalize_OnClick(object sender, RoutedEventArgs e)
        {
            int numberOfPixels = _image.Width*_image.Height;

            ImageStatistics rgbStatistics = new ImageStatistics((Bitmap)_image);
            int[] histogramR = rgbStatistics.Red.Values;
            int[] histogramG = rgbStatistics.Green.Values;
            int[] histogramB = rgbStatistics.Blue.Values;

            // calculate new intensity levels
            byte[] equalizedHistogramR = EqualizeHistogram(histogramR, numberOfPixels);
            byte[] equalizedHistogramG = EqualizeHistogram(histogramG, numberOfPixels);
            byte[] equalizedHistogramB = EqualizeHistogram(histogramB, numberOfPixels);

            // update pixels' intensities
            Bitmap b = (Bitmap) _image;
            for ( int y = 0; y < _image.Height; y++ )
            {
                for ( int x = 0; x < _image.Width; x++ )
                {
                    Color c = b.GetPixel(x, y);
                    b.SetPixel(x, y, Color.FromArgb(
                        equalizedHistogramR[c.R], equalizedHistogramG[c.G], equalizedHistogramB[c.B]));
                }
            }
            _image = b;
            ShowImage(_image);
            histogram.Image = _image;
        }

        // Histogram 
        private byte[] EqualizeHistogram(int[] h, long numPixel)
        {
            byte[] equalizedHistogram = new byte[256];
            float coeffitient = 255.0f / numPixel;

            // calculate the first value
            float prev = h[0] * coeffitient;
            equalizedHistogram[0] = (byte)prev;

            // calcualte the rest of values
            for (int i = 1; i < 256; i++)
            {
                prev += h[i] * coeffitient;
                equalizedHistogram[i] = (byte)prev;
            }

            return equalizedHistogram;
        } 
    }
}
