using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace ImageProcessing
{
    public static class ImageAdjuster
    {
        public static Bitmap ToGrayscale(Image image)
        {
            ImageAttributes imageAttributes = GetGrayscaleAttributes();

            return applyAttributes(image, imageAttributes);
        }

        public static Bitmap ChangeSaturation(Image image, float rate)
        {
            ImageAttributes imageAttributes = GetSaturationAttributes(rate);

            return applyAttributes(image, imageAttributes);
        }

        public static Bitmap ChangeSaturation(Image image, Rectangle region, float rate)
        {
            ImageAttributes imageAttributes = GetSaturationAttributes(rate);

            return applyAttributes(image, region, imageAttributes);
        }

        public static Bitmap ChangeContrast(Image image, float rate)
        {
            ImageAttributes imageAttributes = GetContrastAttributes(rate);

            return applyAttributes(image, imageAttributes);
        }

        public static Bitmap ChangeContrast(Image image, Rectangle region, float rate)
        {
            ImageAttributes imageAttributes = GetContrastAttributes(rate);

            return applyAttributes(image, region, imageAttributes);
        }


        public static Bitmap ChangeBrightness(Image image, float rate)
        {
            ImageAttributes imageAttributes = GetBrightnessAttributes(rate);

            return applyAttributes(image, imageAttributes);
        }

        public static Bitmap ChangeBrightness(Image image, Rectangle region, float rate)
        {
            ImageAttributes imageAttributes = GetBrightnessAttributes(rate);

            return applyAttributes(image, region, imageAttributes);
        }

        public static Bitmap ChangeColour(Image image, float redRate, float greenRate, float blueRate)
        {
            ImageAttributes imageAttributes = GetColourAttributes(redRate, greenRate, blueRate);

            return applyAttributes(image, imageAttributes);
        }

        public static Bitmap ChangeColour(Image image, Rectangle region, float redRate, float greenRate, float blueRate)
        {
            ImageAttributes imageAttributes = GetColourAttributes(redRate, greenRate, blueRate);

            return applyAttributes(image, region, imageAttributes);
        }

        private static ImageAttributes GetSaturationAttributes(float rate)
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            ColorMatrix colorMatrix = new ColorMatrix(GetSaturationMatrix(rate));

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        private static float _lumR = (float)0.3086;
        private static float _lumG = (float)0.6094;
        private static float _lumB = (float)0.0820;

        private static ImageAttributes GetGrayscaleAttributes()
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            ColorMatrix colorMatrix = new ColorMatrix(GetGrayscaleMatrix());

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        private static float[][] GetGrayscaleMatrix()
        {
            float[][] matrix = {
                                new float[]{_lumR,  _lumR,  _lumR,  0, 0},
                                new float[]{_lumG,  _lumG,  _lumG,  0, 0}, 
                                new float[]{_lumB,  _lumB,  _lumB,  0, 0}, 
                                new float[]{0,      0,      0,      1, 0}, 
                                new float[]{0,      0,      0,      0, 1} 
            };
            return matrix;
        }

        private static float[][] GetSaturationMatrix(float rate)
        {
            float sr = (1 - rate) * _lumR;
            float sg = (1 - rate) * _lumG;
            float sb = (1 - rate) * _lumB;
            float[][] matrix = {
                                new float[]{sr + rate,  sr,         sr,         0, 0},
                                new float[]{sg,         sg + rate,  sg,         0, 0}, 
                                new float[]{sb,         sb,         sb + rate,  0, 0}, 
                                new float[]{0,          0,          0,          1, 0}, 
                                new float[]{0,          0,          0,          0, 1} 
            };
            return matrix;
        }

        private static ImageAttributes GetContrastAttributes(float rate)
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            ColorMatrix colorMatrix = new ColorMatrix(GetContrastMatrix(rate));

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        private static float[][] GetContrastMatrix(float rate)
        {
            var t = (1f - rate) * 0.5f;
            float[][] matrix = { 
                                new float[] {rate,  0,     0,     0,  0},        // red scaling factor
                                new float[] {0,     rate,  0,     0,  0},        // green scaling factor
                                new float[] {0,     0,     rate,  0,  0},        // blue scaling factor
                                new float[] {0,     0,     0,     1,  0},        // alpha scaling factor
                                new float[] {t,     t,     t,     0,  1}         // additive
                          };
            return matrix;
        }

        private static ImageAttributes GetBrightnessAttributes(float rate)
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            ColorMatrix colorMatrix = new ColorMatrix(GetBrightnessMatrix(rate));

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        private static float[][] GetBrightnessMatrix(float rate)
        {
            float[][] matrix = { 
                                new float[] {1,     0,     0,     0,  0},        // red scaling factor
                                new float[] {0,     1,     0,     0,  0},        // green scaling factor
                                new float[] {0,     0,     1,     0,  0},        // blue scaling factor
                                new float[] {0,     0,     0,     1,  0},        // alpha scaling factor
                                new float[] {rate,  rate,  rate,  0,  1}         // additive
                            };
            return matrix;
        }

        private static ImageAttributes GetColourAttributes(float redRate, float greenRate, float blueRate)
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            ColorMatrix colorMatrix = new ColorMatrix(GetColourMatrix(redRate, greenRate, blueRate));

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        private static float[][] GetColourMatrix(float redRate, float greenRate, float blueRate)
        {
            float[][] matrix = { 
                                new float[] {redRate,   0,          0,         0,  0},        // red scaling factor
                                new float[] {0,         greenRate,  0,         0,  0},        // green scaling factor
                                new float[] {0,         0,          blueRate,  0,  0},        // blue scaling factor
                                new float[] {0,         0,          0,         1,  0},        // alpha scaling factor
                                new float[] {0,         0,          0,         0,  1}         // additive
                          };
            return matrix;
        }

        private static Bitmap applyAttributes(Image image, ImageAttributes attributes)
        {
            int width = image.Width;
            int height = image.Height;

            Graphics g = default(Graphics);

            Bitmap resultImage = new Bitmap(width, height);
            g = Graphics.FromImage(resultImage);
            Rectangle region = new Rectangle(0, 0, width + 1, height + 1);
            g.DrawImage(image, region, 0, 0, width + 1, height + 1, GraphicsUnit.Pixel, attributes);

            return resultImage;
        }

        private static Bitmap applyAttributes(Image image, Rectangle region, ImageAttributes attributes)
        {
            int width = image.Width;
            int height = image.Height;

            Graphics g = default(Graphics);

            Bitmap resultImage = new Bitmap(width, height);
            g = Graphics.FromImage(resultImage);
            Rectangle fullRegion = new Rectangle(0, 0, width + 1, height + 1);
            g.DrawImage(image, fullRegion, 0, 0, width + 1, height + 1, GraphicsUnit.Pixel);
            g.DrawImage(image, region, region.X, region.Y, region.Width, region.Height,
                GraphicsUnit.Pixel, attributes);

            return resultImage;
        }
    }
}
