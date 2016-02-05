using ImageEditor.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageEditor.Model
{
    public class Filters
    {
        private int maxValue;

        public Filters(int maxValue)
        {
            this.maxValue = maxValue;
        }

        public int Brightness { get; set; }
        public int Contrast { get; set; }
        public int Saturation { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public float SaturationRate
        {
            get
            {
                int value = Saturation;
                if (value > 0)
                    value *= 10;
                return (maxValue + value) / maxValue;
            }
        }

        public float BrightnessRate
        {
            get
            {
                return Brightness / maxValue;
            }
        }

        public float ContrastRate
        {
            get
            {
                return GetExponentialRate(Contrast);
            }
        }

        public float RedRate
        {
            get
            {
                return GetExponentialRate(Red);
            }
        }

        public float GreenRate
        {
            get
            {
                return GetExponentialRate(Green);
            }
        }

        public float BlueRate
        {
            get
            {
                return GetExponentialRate(Blue);
            }
        }

        private float GetExponentialRate(int value)
        {
            return (float)(Math.Pow(Math.E, (double)(maxValue + 1 + value * 2) / maxValue) / Math.E);
        }
    }
}
