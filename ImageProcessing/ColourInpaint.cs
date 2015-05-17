using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;

namespace ImageProcessing
{
    public class ColourInpaint
    {
        public static void Inpaint(Image image, Image maskImage, int lbpWindowSize, int blockSize)
        {
            Bitmap mask = (Bitmap)maskImage;
            int height = image.Height;
            int width = image.Width;
            int pixelSize = 4;
            
            Bitmap bmp = (Bitmap)image;
            Bitmap src = LocalBinaryPattern.LBP(bmp, lbpWindowSize);

//            BitmapData sBitmapData = sBitmap.LockBits(region, ImageLockMode.ReadOnly, image.PixelFormat);
//            IntPtr ptr = sBitmapData.Scan0;
//            int stride = sBitmapData.Stride;
//            int offset = stride - region.Width * pixelSize;
//            int bytes = Math.Abs(sBitmapData.Stride) * image.Height;
//            byte[] source = new byte[bytes];
//            System.Runtime.InteropServices.Marshal.Copy(ptr, source, 0, bytes);
//
//            BitmapData bmpData = bitmap.LockBits(region, ImageLockMode.ReadWrite, bitmap.PixelFormat);
//            ptr = bmpData.Scan0;
//            byte[] destination = new byte[bytes];
//            System.Runtime.InteropServices.Marshal.Copy(ptr, destination, 0, bytes);

//            for (int y = 0; y < height; y++)
//            {
//                for (int x = 0; x < width; x++)
//                {
//                    int i = y * stride + x * pixelSize;
//
//                    Color c = mask.GetPixel(x, y);// Extract the color of a pixel from mask (p) 
//                    int rd = c.R; int gr = c.G; int bl = c.B;// extract the red,green, blue components from the color.
//                    int ti = -1, tj = -1;
//                    double dst = 99999999999999.0;
//                    //4. check if the pixel is white ( that means marked pixel in source)
//                    if ((rd == 255) && (gr == 255) && (bl == 255))
//                    {
//                        //5. Generate the neighbors List
//                        List<int[]> Nbrs = new List<int[]>();
//                        for (int y1 = y - blockSize; y1 < y + blockSize; y1++)
//                        {
//                            if (y1 < 0)
//                                continue;
//                            if (y1 >= height)
//                                break;
//                            for (int x1 = x; x1 < x + blockSize; x1++)
//                            {
//                                if (x1 < 0)
//                                continue;
//
//                                if (x1 < height)
//                                {
//                                    int j = y * stride + x * pixelSize;
//                                    Color c1 = src.GetPixel(x1, y1); // Extract the color of a pixel from LBP image 
//                                    int rd1 = c1.R;
//                                    int gr1 = c1.G;
//                                    int bl1 = c1.B; // extract the red,green, blue components from the color.
//                                    Color c2 = mask.GetPixel(x1, y1); // Extract the color of a mask pixel 
//                                    // remember list can not contain a pixel which also is within mask region
//                                    int rd2 = c2.R;
//                                    int gr2 = c2.G;
//                                    int bl2 = c2.B; // extract the red,green, blue components from the color.
//                                    // form the list with non marked pixel
//                                    if ((rd2 == 0) && (gr2 == 0) && (bl2 == 0))
//                                    {
//                                        // add first pixel as it is, as there is nothing to compare for
//                                        if (Nbrs.Count == 0)
//                                        {
//                                            Nbrs.Add(new int[] {y1, x1});
//                                        }
//                                        else
//                                        {
//                                            double d = 0;
//                                            //6. calculate mean distance of the current pixel with all neighbors
//                                            for (int k = 0; k < Nbrs.Count; k++)
//                                            {
//                                                int[] pos = Nbrs[k];
//                                                d = d + Math.Abs(bmp.GetPixel(pos[1], pos[0]).R - rd2);
//                                            }
//                                            d = d/(double) Nbrs.Count;
//                                            // 7. update ps value which will be used to replace p in original image
//                                            if (d < dst)
//                                            {
//                                                dst = d;
//                                                ti = y1;
//                                                tj = x1;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        //8. replace p with ps in the actual image
//                        bmp.SetPixel(x, y, bmp.GetPixel(tj, ti));
//                        System.Threading.Thread.Sleep(10);
//                    }
//                }
//            }
        }  


        public static Bitmap ObtainMask(Bitmap image, Color marker)
        {
            Bitmap bmp = (Bitmap)image.Clone();
            int height = bmp.Height;
            int width = bmp.Width;
            int pixelSize = 4;
            Bitmap mask = new Bitmap(width, height);
            var b = (Bitmap) image;
            BitmapData bmpData = b.LockBits(new Rectangle(0,0, width,height), ImageLockMode.ReadOnly, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            BitmapData maskData = mask.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, mask.PixelFormat);
            ptr = maskData.Scan0;
            byte[] destination = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, destination, 0, bytes);
            int bnd = 3;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * pixelSize;
                    if ((rgbValues[i] == marker.R) && (rgbValues[i] == marker.G) && (rgbValues[i] == marker.B))
                    {
                        destination[i] = 255;
                        destination[i + 1] = 255;
                        destination[i + 2] = 255;

                        for (int ib = y - bnd; ib < y + bnd; ib++)
                        {
                            if (ib < 0)
                                continue;
                            if (ib >= height)
                                break;
                            for (int jb = x - bnd; jb < x + bnd; jb++)
                            {
                                if ( jb < 0 )
                                continue;

                                if ( jb < width )
                                {
                                    int index = ib * stride + jb * pixelSize;
                                    destination[index] = 255;
                                    destination[index + 1] = 255;
                                    destination[index + 2] = 255;
                                }
                            }
                        }
                    }
                    else
                    {
                        destination[i] = 0;
                        destination[i + 1] = 0;
                        destination[i + 2] = 0;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(destination, 0, ptr, bytes);
            mask.UnlockBits(maskData);
            b.UnlockBits(bmpData);
            return mask;
        } 
    }
}
