using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using ImageEditor.Annotations;

namespace ImageEditor.Model
{
    public class EditableImage : INotifyPropertyChanged
    {
        private string path ;
        private Bitmap source;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                Format = GetImageFormat(value);
                Source = GetFormattedBitmap(value);
            }
        }

        public EditableImage(string path)
        {
            Path = path;
        }

        public Bitmap Source
        {
            get { return source; } 
            set { source = value;  OnPropertyChanged();}
        }

        public ImageFormat Format { get; set; }
        public int Height { get { return source.Height; } }
        public int Width { get { return source.Width; }  }

        public Rectangle GetFullRectangle()
        {
            return new Rectangle(0, 0, Width, Height);
        }

        public void Resize(int newWidth, int newHeight)
        {
            Source = new Bitmap(source, newWidth, newHeight);
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

        public void FillRegion(Rectangle region, Color color)
        {
            Graphics g = Graphics.FromImage(source);
            g.FillRectangle(new SolidBrush(color), region);
            OnPropertyChanged("Source");
        }

        public Bitmap GetRegion(Rectangle region)
        {
            Bitmap b = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(source, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            return b;
        }
        public void SetRegion(Rectangle region, Bitmap newSource)
        {
            Graphics g = Graphics.FromImage(source);
            g.DrawImage(newSource, region, 0, 0, newSource.Width, newSource.Height, GraphicsUnit.Pixel);
            OnPropertyChanged("Source");
        }

        public void Crop(Rectangle region)
        {
            Source = GetRegion(region);
        }

        public EditableImage Clone()
        {
            EditableImage result = (EditableImage)this.MemberwiseClone();
            result.Source = new Bitmap(Source); ;
            return result;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private Bitmap GetFormattedBitmap(string path)
        {
            Bitmap result = new Bitmap(path);
            if (result.PixelFormat != PixelFormat.Format32bppArgb)
                return result.Clone(new Rectangle(0, 0, result.Width, result.Height), PixelFormat.Format32bppArgb);
            else
                return new Bitmap(path);
        }

        private ImageFormat GetImageFormat(string imagePath)
        {
            ImageFormat result = ImageFormat.Png;
            string extention = System.IO.Path.GetExtension(imagePath);
            if (extention == ".jpg")
            {
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
    }
}
