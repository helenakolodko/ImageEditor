using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Model
{
    public class Inpainting
    {
        public bool CreateMask { get; set; }

        public int LbpWindowSize { get; set; }

        public int InpaintBlockSize { get; set; }
    }
}
