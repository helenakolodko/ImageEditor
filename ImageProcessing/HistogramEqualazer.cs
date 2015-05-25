using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using AForge.Imaging;

namespace ImageProcessing
{
    static public class HistogramEqualazer
    {
        public static Bitmap Equalize(Bitmap image, Rectangle region)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = image.Width - 1;
            int stopY = image.Height - 1;
            if (startX + region.Width < image.Width)
            {
                stopX = startX + region.Width;
            }
            if (startY + region.Height < image.Height)
            {
                stopY = startX + region.Height;
            }

            int numberOfPixels = (stopX - startX) * (stopY - startY);

            Bitmap bitmap = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(image, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            ImageStatistics rgbStatistics = new ImageStatistics(bitmap);

            byte[] equalizedHistogramR = EqualizeHistogram(rgbStatistics.Red.Values, numberOfPixels);
            byte[] equalizedHistogramG = EqualizeHistogram(rgbStatistics.Green.Values, numberOfPixels);
            byte[] equalizedHistogramB = EqualizeHistogram(rgbStatistics.Blue.Values, numberOfPixels);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            Parallel.For(startY, stopY, y =>
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y * stride + x * 4;
                    rgbValues[i] = equalizedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = equalizedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = equalizedHistogramB[rgbValues[i + 2]];
                }
            });

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

        public static Bitmap Stretch(Bitmap image, Rectangle region)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = image.Width - 1;
            int stopY = image.Height - 1;
            if (startX + region.Width < image.Width)
            {
                stopX = startX + region.Width;
            }
            if (startY + region.Height < image.Height)
            {
                stopY = startX + region.Height;
            }

            Bitmap bitmap = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(image, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            ImageStatistics rgbStatistics = new ImageStatistics(bitmap);

            byte[] stretchedHistogramR = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);
            byte[] stretchedHistogramG = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);
            byte[] stretchedHistogramB = StretchHistogram(rgbStatistics.Red.Max, rgbStatistics.Red.Min);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            Parallel.For(startY, stopY, y =>
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y*stride + x*4;
                    rgbValues[i] = stretchedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = stretchedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = stretchedHistogramB[rgbValues[i + 2]];
                }
            });

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
        
        public static Bitmap Squeeze(Bitmap image, Rectangle region, int min, int max)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = image.Width - 1;
            int stopY = image.Height - 1;
            if (startX + region.Width < image.Width)
            {
                stopX = startX + region.Width;
            }
            if (startY + region.Height < image.Height)
            {
                stopY = startX + region.Height;
            }

            byte[] squeezedHistogramR = SqueezeHistogram(max, min);
            byte[] squeezedHistogramG = SqueezeHistogram(max, min);
            byte[] squeezedHistogramB = SqueezeHistogram(max, min);

            var b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            Parallel.For(startY, stopY, y =>
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y*stride + x*4;
                    rgbValues[i] = squeezedHistogramR[rgbValues[i]];
                    rgbValues[i + 1] = squeezedHistogramG[rgbValues[i + 1]];
                    rgbValues[i + 2] = squeezedHistogramB[rgbValues[i + 2]];
                }
            });

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
