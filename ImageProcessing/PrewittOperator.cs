using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class PrewittOperator
    {
        public static Bitmap Process(Bitmap image, Rectangle region)
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

                    //rgbValues[i] = curve[rgbValues[i]];
                    //rgbValues[i + 1] = curve[rgbValues[i + 1]];
                    //rgbValues[i + 2] = curve[rgbValues[i + 2]];
                }
            });

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }
    }
}
