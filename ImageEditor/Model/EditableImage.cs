using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageEditor.Model
{
    public class EditableImage
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
                    Bitmap b = (Bitmap)_source;
                    _source = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
                }
            }
        }

        private Bitmap _source;

        public EditableImage(string path)
        {
            Path = path;
        }

        public Bitmap Source { get { return _source; } set { _source = value; } }

        public ImageFormat Format { get; set; }

        public int Height { get { return _source.Height; } }
        public int Width { get { return _source.Width; }  }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public void SetBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

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
            _source = new Bitmap(_source, newWidth, newHeight);
        }

        public void FlipHorisontal()
        {
            _source.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        public void FlipVertical()
        {
            _source.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public void RotateClockwise()
        {
            _source.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public void RotateAntiClockwise()
        {
            _source.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        public void ChangeBrightness(float rate, Rectangle region)
        {
            
        }

        public void ChangeContrast(float rate, Rectangle region)
        {

        }

        public void ChangeSaturation(float rate, Rectangle region)
        {

        }

        public void ChangeColour(float redRate, float greenRate, float blueRate, Rectangle region)
        {

        }

        public void FillRegion(Rectangle region, Color color)
        {
            Graphics g = Graphics.FromImage(_source);
            g.FillRectangle(new SolidBrush(color), region);
        }


        public void GetRegion(Rectangle region)
        {
            
        }
        public void SetRegion(Rectangle region, Bitmap newSource)
        {

        }
    }
}
