using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AForge.Imaging;
using Image = System.Drawing.Image;

namespace ImageProcessing
{
    static public class HistogramEqualazer
    {
        public static Image Equalize(Image image, Rectangle region)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = startX + region.Width;
            int stopY = startY + region.Height;

            int numberOfPixels = (stopX - startX) * (stopY - startY);

            Bitmap bitmap = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(image, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            ImageStatistics rgbStatistics = new ImageStatistics(bitmap);

            byte[] equalizedHistogramR = EqualizeHistogram(rgbStatistics.Red.Values, numberOfPixels);
            byte[] equalizedHistogramG = EqualizeHistogram(rgbStatistics.Green.Values, numberOfPixels);
            byte[] equalizedHistogramB = EqualizeHistogram(rgbStatistics.Blue.Values, numberOfPixels);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(region, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = startY; y < stopY; y++)
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y * stride + x * 4;
                    rgbValues[i] = equalizedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = equalizedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = equalizedHistogramB[rgbValues[i + 2]];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }

        private static byte[] EqualizeHistogram(int[] historgam, long numPixel)
        {
            byte[] equalizedHistogram = new byte[256];
            float coeffitient = 255.0f / numPixel;

            float prev = historgam[0] * coeffitient;
            equalizedHistogram[0] = (byte)prev;

            for (int i = 1; i < 256; i++)
            {
                prev += historgam[i] * coeffitient;
                equalizedHistogram[i] = (byte)prev;
            }

            return equalizedHistogram;
        }

        public static Image Stretch(Image image, Rectangle region)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = startX + region.Width;
            int stopY = startY + region.Height;
            int width = image.Width;

            int numberOfPixels = (stopX - startX) * (stopY - startY);

            Bitmap bitmap = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(image, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            ImageStatistics rgbStatistics = new ImageStatistics(bitmap);

            byte[] stretchedHistogramR = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);
            byte[] stretchedHistogramG = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);
            byte[] stretchedHistogramB = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(region, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = startY; y < stopY; y++)
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y * stride + x * 4;
                    rgbValues[i] = stretchedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = stretchedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = stretchedHistogramB[rgbValues[i + 2]];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }

        private static byte[] StretchHistogram(int max, int min)
        {
            byte[] stretchedHistogram = new byte[256];
            int delta = max - min;

            for (int i = 0; i < 256; i++)
            {
                stretchedHistogram[i] = (byte)((double)(i - min) / delta * 255.0);
            }

            return stretchedHistogram;
        }

        public static Image Squeeze(Image image, Rectangle region, int min, int max)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = startX + region.Width;
            int stopY = startY + region.Height;
            int width = image.Width;

            int numberOfPixels = (stopX - startX) * (stopY - startY);

            byte[] squeezedHistogramR = SqueezeHistogram(max, min);
            byte[] squeezedHistogramG = SqueezeHistogram(max, min);
            byte[] squeezedHistogramB = SqueezeHistogram(max, min);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(region, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = startY; y < stopY; y++)
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y * stride + x * 4;
                    rgbValues[i] = squeezedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = squeezedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = squeezedHistogramB[rgbValues[i + 2]];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }

        private static byte[] SqueezeHistogram(int max, int min)
        {
            byte[] stretchedHistogram = new byte[256];
            float coefficient = (max - min) / 255f;

            for (int i = 0; i < 256; i++)
            {
                stretchedHistogram[i] = (byte)(min + i * coefficient);
            }

            return stretchedHistogram;
        }
    }
}
