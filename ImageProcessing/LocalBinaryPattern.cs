using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    public static class LocalBinaryPattern
    {
        public static Bitmap LBP(Bitmap image, int windowSize)
        {
            Bitmap bmp = ImageAdjuster.ToGrayscale(image);
            int height = image.Height;
            int width = image.Width;
            int pixelSize = 4;
            double[,] matrix = new double[width, height];
            double max = 0.0;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * pixelSize;
                    matrix[x, y] = 0;

                    if ((y > windowSize) && (x > windowSize) && (y < (height - windowSize)) && (x < (width - windowSize)))
                    {
                        List<int> vals = new List<int>();
                        for (int y1 = y - windowSize; y1 < (y + windowSize); y1++)
                        {
                            if (y1 < 0)
                                continue;
                            if (y1 >= height)
                                break;
                            for (int x1 = x - windowSize; x1 < (x + windowSize); x1++)
                            {
                                if (x1 < 0)
                                continue;

                                if (x1 < height)
                                {
                                    int j = y*stride + x*pixelSize;
                                    int acPixel = rgbValues[i];
                                    int nbrPixel = rgbValues[j];
                                    vals.Add(nbrPixel > acPixel ? 1 : 0);
                                }
                            }
                        }
                        double d1 = Bin2Dec(vals);
                        matrix[x, y] = d1;
                        if (d1 > max)
                        {
                            max = d1;
                        }
                    }
                }
            }
            bmp.UnlockBits(bmpData);
            Bitmap lbp = new Bitmap(width, height);
            lbp = NormalizeLbpMatrix(matrix, lbp, max);
            return lbp;
        }
//
        private static double Bin2Dec(List<int> bin)
        {
            double d = 0;

            for (int i = 0; i < bin.Count; i++)
            {
                d += bin[i] * Math.Pow(2, i);
            }
            return d;
        }

        private static Bitmap NormalizeLbpMatrix(double[,] matrix, Bitmap lbp, double max)
        {
            int height = lbp.Height;
            int width = lbp.Width;
            BitmapData bmpData = lbp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, lbp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int pixelSize = 4;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * pixelSize;
                    double d = matrix[x, y] / max;
                    byte v = (byte)(d * 255);
                    rgbValues[i] = v;
                    rgbValues[i + 1] = v;
                    rgbValues[i + 2] = v;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            lbp.UnlockBits(bmpData);
            return lbp;
        }  
    }
}
