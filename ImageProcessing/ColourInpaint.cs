using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    public class ColourInpaint
    {
        public static Bitmap Inpaint(Bitmap image, Color color, int lbpWindowSize, int blockSize)
        {
            return Inpaint(image, ObtainMaskMatrix(image, color), lbpWindowSize, blockSize);
        }

        public static Bitmap Inpaint(Bitmap image, byte[,] mask, int lbpWindowSize, int blockSize)
        {
            int height = image.Height;
            int width = image.Width;
            
            Bitmap src = LocalBinaryPattern.LBP(image, lbpWindowSize);

            BitmapData srcData = src.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, src.PixelFormat);
            IntPtr ptr = srcData.Scan0;
            int stride = srcData.Stride;
            int bytes = Math.Abs(srcData.Stride) * height;
            byte[] source = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, source, 0, bytes);

            BitmapData destData = image.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, image.PixelFormat);
            ptr = destData.Scan0;
            byte[] destination = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, destination, 0, bytes);
            int pixelSize = 4;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y*stride + x*pixelSize;
                    int t = -1;
                    double dst = Double.MaxValue;
                    if (mask[x, y] == 1)
                    {
                        List<int> Nbrs = new List<int>();
                        for (int y1 = y - blockSize; y1 < y + blockSize; y1++)
                        {
                            if (y1 < 0)
                                continue;
                            if (y1 >= height)
                                break;
                            for (int x1 = x; x1 < x + blockSize; x1++)
                            {
                                if (x1 < 0)
                                    continue;

                                if (x1 < height)
                                {
                                    int j = y1*stride + x1*pixelSize;
                                    if (mask[x1, y1] == 0)
                                    {
                                        if (Nbrs.Count == 0)
                                        {
                                            Nbrs.Add(j);
                                        }
                                        else
                                        {
                                            double d = 0;
                                            for (int k = 0; k < Nbrs.Count; k++)
                                            {
                                                int pos = Nbrs[k];
                                                d = d + Math.Abs(destination[pos] - source[j]);
                                            }
                                            d = d/(double) Nbrs.Count;
                                            if (d < dst)
                                            {
                                                dst = d;
                                                t = j;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (t >= 0 )
                        {
                            destination[i] = destination[t];
                            destination[i + 1] = destination[t + 1];
                            destination[i + 2] = destination[t + 2];    
                        }
                        
                    }
                }
            } 
            System.Runtime.InteropServices.Marshal.Copy(destination, 0, ptr, bytes);
            image.UnlockBits(destData);
            src.UnlockBits(srcData);
            return image;
        }


        public static byte[,] ObtainMaskMatrix(Bitmap image, Color marker)
        {
            int height = image.Height;
            int width = image.Width;
            int pixelSize = 4;
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            byte[,] mask = new byte[width, height];
            
            int bnd = 1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * pixelSize;
                    if ((rgbValues[i] == marker.B) && (rgbValues[i + 1] == marker.G) && (rgbValues[i + 2] == marker.R))
                    {
                        mask[x, y] = 1;

                        for (int ib = y - bnd; ib < y + bnd; ib++)
                        {
                            if (ib < 0)
                                continue;
                            if (ib >= height)
                                break;
                            for (int jb = x - bnd; jb < x + bnd; jb++)
                            {
                                if (jb < 0)
                                    continue;

                                if (jb < width)
                                {
                                    mask[x, y] = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        mask[x, y] = 0;
                    }
                }
            }

            image.UnlockBits(bmpData);
            return mask;
        }

        public static byte[,] ObtainMaskMatrix(Bitmap image)
        {
            int height = image.Height;
            int width = image.Width;
            int pixelSize = 4;
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            byte[,] mask = new byte[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * pixelSize;
                    if ((rgbValues[i] == 255) && (rgbValues[i + 1] == 255) && (rgbValues[i + 2] == 255))
                    {
                        mask[x, y] = 1;
                    }
                    else
                    {
                        mask[x, y] = 0;
                    }
                }
            }

            image.UnlockBits(bmpData);
            return mask;
        } 
    }
}
