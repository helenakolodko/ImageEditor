using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public struct CieXyzPixel
    {
        float X;
        float Y;
        float Z;

        public CieXyzPixel(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

       
    }

    public static class ColourSpaceConverter
    {
        private static CieXyzPixel CieXyzWhite = new CieXyzPixel(95.047f, 100.000f, 108.883f);

        public static CieXyzPixel[] ToCieXyz(byte[] rgbValues, int length)
        {
            CieXyzPixel[] result = new CieXyzPixel[length];

            return result;
        }

        private static CieXyzPixel FromRgb(byte red, byte green, byte blue)
        {
            float rLinear = red / 255.0f;
            float gLinear = green / 255.0f;
            float bLinear = blue / 255.0f;

            float r = (rLinear > 0.04045f) ? (float)Math.Pow((rLinear + 0.055f) / (1.055), 2.2) : (rLinear / 12.92f);
            float g = (gLinear > 0.04045f) ? (float)Math.Pow((gLinear + 0.055f) / (1.055), 2.2) : (gLinear / 12.92f);
            float b = (bLinear > 0.04045f) ? (float)Math.Pow((bLinear + 0.055f) / (1.055), 2.2) : (bLinear / 12.92f);

            return new CieXyzPixel(r * 0.4124f + g * 0.3576f + b * 0.1805f,
                r * 0.2126f + g * 0.7152f + b * 0.0722f,
                r * 0.0193f + g * 0.1192f + b * 0.9505f);
        }
    }
}
