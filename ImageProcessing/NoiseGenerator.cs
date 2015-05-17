﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Math.Random;

namespace ImageProcessing
{
    public static class NoiseGenerator
    {
        public static Image SaltAndPapper(Image image, Rectangle region, int amount)
        {
            int startX = region.Left;
            int startY = region.Top;
            int width = region.Width;
            int height = region.Height;

            int noisyPixels = (int)((width * height * amount) / 100);
            byte[] values = new byte[2] { 0, 255 };

            Random rand = new Random();
            Bitmap b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(region, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int j = 0; j < noisyPixels; j++)
            {
                int x = startX + rand.Next(width);
                int y = startY + rand.Next(height);
                int i = y * stride + x * 4;
                byte value = values[rand.Next(2)];
                rgbValues[i] = value;
                rgbValues[i + 1] = value;
                rgbValues[i + 2] = value;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }

        public static Image Additive(Image image, Rectangle region, int amount)
        {
            int startX = region.Left;
            int startY = region.Top;
            int width = region.Width;
            int height = region.Height;
            int stopX = startX + width;
            int stopY = startY + height;

            int noisyPixels = (int)((width * height * amount) / 100);

            Random rand = new Random();
            Bitmap b = new Bitmap(image);
            BitmapData bmpData = b.LockBits(region, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;

            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            IRandomNumberGenerator generator = new UniformGenerator(new Range(-amount, amount));
            for (int y = startY; y < stopY; y++)
            {
                for (int x = startX; x < stopX; x++)
                {
                    int i = y * stride + x * 4;
                    rgbValues[i] = (byte)Math.Max(0, Math.Min(255, rgbValues[i] + generator.Next()));
                    rgbValues[i + 1] = (byte)Math.Max(0, Math.Min(255, rgbValues[i + 1] + generator.Next()));
                    rgbValues[i + 2] = (byte)Math.Max(0, Math.Min(255, rgbValues[i + 2] + generator.Next()));
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);

            return b;
        }
    }
}
