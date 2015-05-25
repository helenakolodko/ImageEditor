using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;

namespace ImageProcessing
{
    public static class NoiseReducer
    {
        public static Bitmap Median(Bitmap image, Rectangle region, int size)
        {
            int startX = region.Left;
            int startY = region.Top;
            int stopX = image.Width;
            int stopY = image.Height;
            if (startX + region.Width < image.Width)
            {
                stopX = startX + region.Width;
            }
            if (startY + region.Height < image.Height)
            {
                stopY = startX + region.Height;
            }
            int pixelSize = 4;
            int c;
            int radius = size >> 1;

            byte[] r = new byte[size * size];
            byte[] g = new byte[size * size];
            byte[] b = new byte[size * size];
            int i, j, k;

            Bitmap sBitmap = image;
            var bitmap = new Bitmap(image);
            BitmapData sBitmapData = sBitmap.LockBits(new Rectangle(0, 0, sBitmap.Width, sBitmap.Height),
                ImageLockMode.ReadOnly, image.PixelFormat);
            IntPtr ptr = sBitmapData.Scan0;
            int stride = sBitmapData.Stride;
            int offset = stride - region.Width * pixelSize;
            int bytes = Math.Abs(sBitmapData.Stride) * image.Height;
            byte[] source = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, source, 0, bytes);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            ptr = bmpData.Scan0;
            byte[] destination = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, destination, 0, bytes);

            int sourceOffset = startY * stride + startX * pixelSize;
            int destinationOffset = sourceOffset;

            for ( int y = startY; y < stopY; y++ )
            {
                for ( int x = startX; x < stopX; x++, sourceOffset += pixelSize, destinationOffset += pixelSize)
                {
                    c = 0;
                    for ( i = -radius; i <= radius; i++ )
                    {
                        k = y + i;
                        if ( k < startY )
                            continue;
                        if ( k >= stopY )
                            break;

                        for ( j = -radius; j <= radius; j++ )
                        {
                            k = x + j;
                            if ( k < startX )
                                continue;

                            if ( k < stopX )
                            {
                                int sourceIndex = i * stride + j * pixelSize + sourceOffset;
                                r[c] = source[sourceIndex];
                                g[c] = source[sourceIndex + 1];
                                b[c] = source[sourceIndex + 2];
                                c++;
                            }
                        }
                    }
                    Array.Sort( r, 0, c );
                    Array.Sort( g, 0, c );
                    Array.Sort( b, 0, c );
                    k = c >> 1;
                    destination[destinationOffset] = r[k];
                    destination[destinationOffset + 1] = g[k];
                    destination[destinationOffset + 2] = b[k];
                }
                sourceOffset += offset;
                destinationOffset += offset;
            }
            System.Runtime.InteropServices.Marshal.Copy(destination, 0, ptr, bytes);
            bitmap.UnlockBits(bmpData);
            sBitmap.UnlockBits(sBitmapData);

            return bitmap;
        }

        public static Bitmap Bilateral(Bitmap image, Rectangle region, int kernelSize, int spatialFactor, int colorFactor)
        {
            BilateralSmoothing filter = new BilateralSmoothing
            {
                KernelSize = kernelSize,
                SpatialFactor = spatialFactor,
                ColorFactor = colorFactor,
                EnableParallelProcessing = true
            };

            Bitmap bitmap = new Bitmap(image);
            filter.ApplyInPlace(bitmap, region);
            return bitmap;
        }
    }
}
