namespace ImageEditor.Model
{
    public class Noise
    {
        public Noise()
        {
            Reset();
        }

        public bool SaltAndPapper { get; set; }
        public bool Median { get; set; }
        public int Coverage { get; set; }
        public int MedianRadius { get; set; }
        public int KernelSize { get; set; }
        public int SpatialFactor { get; set; }
        public int ColourFactor { get; set; }

        public void Reset()
        {
            Coverage = 0;
            MedianRadius = 1;
            KernelSize = 3;
            SpatialFactor = 0;
            ColourFactor = 0;
        }
    }
}
