using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImageProcessing
{
    internal class CustomImage
    {
        private Bitmap bitmap;
        private bool read;
        private byte[][] red;
        private byte[][] green;
        private byte[][] blue;

        public CustomImage(Bitmap bitmap)
        {
            this.bitmap = new Bitmap(bitmap);
            Height = bitmap.Height;
            Width = bitmap.Width;

        }

        private void ReadValues()
        {
            BitmapData data =  bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int stride = data.Stride;
            System.IntPtr pointer = data.Scan0;
            int bytes = Math.Abs(data.Stride) * Height;
            byte[] values = new byte[bytes];

            red = new byte[Width][];
            blue = new byte[Width][];
            green = new byte[Width][];

            Parallel.For(0, Width, x =>
                {
                    red[x] = new byte[Height];
                    blue[x] = new byte[Height];
                    green[x] = new byte[Height];
                    for (int y = 0; y < Height; y++)
                    {
                        int i = y * stride + x * 4;
                        red[x][y] = values[i];
                        green[x][y] = values[i + 1];
                        blue[x][y] = values[i + 2];
                    }
                });
            bitmap.UnlockBits(data);
            read = true;
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
    }
}
