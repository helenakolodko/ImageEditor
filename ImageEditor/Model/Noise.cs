using ImageEditor.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Model
{
    public class Noise
    {
        public Noise()
        {
            MedianRadius = 1;
            KernelSize = 3;
        }

        public bool SaltAndPapper { get; set; }
        public bool Median { get; set; }
        public int Coverage { get; set; }
        public int MedianRadius { get; set; }
        public int KernelSize { get; set; }
        public int SpatialFactor { get; set; }
        public int ColourFactor { get; set; }
    }
}
