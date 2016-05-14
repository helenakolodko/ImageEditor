using ImageEditor.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageEditor.Model
{
    public delegate void ValueChanged();
    public class Filters
    {
        private int maxValue;
        private int brightness;
        private int contrast;
        private int saturation;
        private int red;
        private int green;
        private int blue;

        public event ValueChanged ValueChanged;
        public Filters(int maxValue)
        {
            this.maxValue = maxValue;
        }

        public int Brightness { 
            get { return brightness; }
            set { brightness = value; OnValueChanged(); }
        }

        public int Contrast
        {
            get { return contrast; }
            set { contrast = value; OnValueChanged(); }
        }
        public int Saturation
        {
            get { return saturation; }
            set { saturation = value; OnValueChanged(); }
        }
        public int Red
        {
            get { return red; }
            set { red = value; OnValueChanged(); }
        }
        public int Green
        {
            get { return green; }
            set { green = value; OnValueChanged(); }
        }
        public int Blue
        {
            get { return blue; }
            set { blue = value; OnValueChanged(); }
        }

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
                return (float)Brightness / maxValue;
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

        private void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged();
        }

        private float GetExponentialRate(int value)
        {
            return (float)(Math.Pow(Math.E, (double)(maxValue + 1 + value * 2) / maxValue) / Math.E);
        }
    }
}
