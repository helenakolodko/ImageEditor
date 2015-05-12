using System.Drawing;
using AForge.Imaging;

namespace ImageProcessing
{
    public class ImageHistogram
    {

        public int[] RedValues { get; private set; }
        public int[] GreenValues { get; private set; }
        public int[] BlueValues { get; private set; }

        private Bitmap _image;

        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                GetHistogram();
            }
        }

        private void GetHistogram()
        {
            ImageStatistics rgbStatistics = new ImageStatistics(_image);
            AForge.Math.Histogram h = rgbStatistics.Red;
            RedValues = h.Values;
            h = rgbStatistics.Green;
            GreenValues = h.Values;
            h = rgbStatistics.Blue;
            BlueValues = h.Values;
        }

        public ImageHistogram(Bitmap image)
        {
            Image = image;
        }

        public void Equilize()
        {

        }


    }
}
