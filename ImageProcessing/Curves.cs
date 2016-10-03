using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class Curves
    {
        public static Bitmap ProcessD(Bitmap image, int G, int F, Rectangle region)
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

            var curve = GetDCurve(G, F);

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
                    rgbValues[i] = curve[rgbValues[i]];
                    rgbValues[i + 1] = curve[rgbValues[i + 1]];
                    rgbValues[i + 2] = curve[rgbValues[i + 2]];
                }
            });

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }

        public static Bitmap ProcessE(Bitmap image, int G, int F, Rectangle region)
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

            var curve = GetECurve(G, F);

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
                    rgbValues[i] = curve[rgbValues[i]];
                    rgbValues[i + 1] = curve[rgbValues[i + 1]];
                    rgbValues[i + 2] = curve[rgbValues[i + 2]];
                }
            });

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }



        private static byte[] GetDCurve(int G, int F)
        {
            double m = (F - G) / (double)255;
            var result = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = (byte)Math.Round(m * i + G);
            }
            return result;
        }

        private static byte[] GetECurve(int G, int F)
        {
            double m = (double)255 / (F - G);
            var result = new byte[256];
            for (int i = G; i < F; i++)
            {
                result[i] = (byte)Math.Round(m * i);
            }
            for (int i = F; i < 256; i++)
            {
                result[i] = 255;
            }
            return result;
        }
    }
}
