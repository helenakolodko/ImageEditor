using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using ImageEditor.Annotations;
using ImageProcessing;

namespace ImageEditor.Model
{
    public class EditableImage : INotifyPropertyChanged
    {
        private string _path ;

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                Format = GetImageFormat(value);
                _source = new Bitmap(value);
                if (_source.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    Bitmap b = _source;
                    _source = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);

                }
                OnPropertyChanged("Source");
            }
        }

        private Bitmap _source;

        public EditableImage(string path)
        {
            Path = path;
        }

        public Bitmap Source
        {
            get { return _source; } 
            set { _source = value;  OnPropertyChanged();}
        }

        public ImageFormat Format { get; set; }

        public int Height { get { return _source.Height; } }
        public int Width { get { return _source.Width; }  }

        private ImageFormat GetImageFormat(string imagePath)
        {
            ImageFormat result = ImageFormat.Png;
            string extention = System.IO.Path.GetExtension(imagePath);
            if(extention == ".jpg"){
                result = ImageFormat.Jpeg;
            }
            else if (extention == ".bmp")
            {
                result = ImageFormat.Bmp;
            }
            else if (extention == ".gif")
            {
                result = ImageFormat.Gif;
            }
            return result;
        }

        public BitmapImage GetImageToDisplay()
        {
            MemoryStream ms = new MemoryStream();
            _source.Save(ms, Format);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public void Resize(int newWidth, int newHeight)
        {
            Source = new Bitmap(_source, newWidth, newHeight);
        }

        public void FlipHorisontal()
        {
            Source.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public void FlipVertical()
        {
            Source.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        public void RotateClockwise()
        {
            Source.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public void RotateAntiClockwise()
        {
            Source.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        public void ChangeBrightness(float rate, Rectangle region)
        {
            Source = ImageAdjuster.ChangeBrightness(_source, region, rate);
        }

        public void ChangeContrast(float rate, Rectangle region)
        {
            Source = ImageAdjuster.ChangeContrast(_source, region, rate);
        }

        public void ChangeSaturation(float rate, Rectangle region)
        {
            Source = ImageAdjuster.ChangeSaturation(_source, region, rate);
        }

        public void ChangeColour(float redRate, float greenRate, float blueRate, Rectangle region)
        {
            Source = ImageAdjuster.ChangeColour(_source, region, redRate, greenRate, blueRate);
        }

//        public void DrawLine

//        public void DrawPath

//        public void DrawBrush
        

        public void FillRegion(Rectangle region, Color color)
        {
            Graphics g = Graphics.FromImage(_source);
            g.FillRectangle(new SolidBrush(color), region);
            OnPropertyChanged("Source");
        }


        public Bitmap GetRegion(Rectangle region)
        {
            Bitmap b = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(_source, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            return b;
        }
        public void SetRegion(Rectangle region, Bitmap newSource)
        {
            Graphics g = Graphics.FromImage(_source);
            g.DrawImage(newSource, region, 0, 0, newSource.Width, newSource.Height, GraphicsUnit.Pixel);
            OnPropertyChanged("Source");
        }

        public void Crop(Rectangle region)
        {
            Source = GetRegion(region);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditableImage Clone()
        {
            EditableImage result = (EditableImage)this.MemberwiseClone();
            result.Source = _source.Clone(new Rectangle(0, 0, _source.Width, _source.Height), _source.PixelFormat);
            return result;
        }
    }
}
