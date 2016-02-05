using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AForge.Imaging;
using ImageEditor.Annotations;
using Point = System.Windows.Point;

namespace ImageEditor.View
{
	public partial class BitmapImageHistogram : INotifyPropertyChanged
	{
        private Bitmap image;
        private PointCollection redCurve;
        private PointCollection greenCurve;
        private PointCollection blueCurve;

        public event PropertyChangedEventHandler PropertyChanged;

        public Bitmap Image
	    {
            get { return (Bitmap)GetValue(ImageProperty); }
	        set { SetValue(ImageProperty, value); }
	    }

        public static readonly DependencyProperty ImageProperty = 
            DependencyProperty.Register("Image", typeof(Bitmap), typeof(BitmapImageHistogram),
            new PropertyMetadata(default(Bitmap), OnImagePropertyChanged));

	    public PointCollection RedCurve
	    {
	        get { return redCurve; }
	        set
	        {
	            redCurve = value;
	            OnPropertyChanged();
	        }
	    }


        public PointCollection GreenCurve
        {
            get { return greenCurve; }
            set
            {
                greenCurve = value;
                OnPropertyChanged();
            }
        }

        public PointCollection BlueCurve
        {
            get { return blueCurve; }
            set
            {
                blueCurve = value;
                OnPropertyChanged();
            }
        }

		public BitmapImageHistogram()
		{
			InitializeComponent();
		}

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetHistogram()
	    {
            ImageStatistics rgbStatistics = new ImageStatistics(Image);
            Point[] red = null;
            Point[] green = null;
            Point[] blue = null; 
            Parallel.Invoke(
                delegate { red = GetPoints(rgbStatistics.Red.Values); },
                delegate { green = GetPoints(rgbStatistics.Green.Values); },
                delegate { blue = GetPoints(rgbStatistics.Blue.Values); });
            RedCurve = new PointCollection(red);
            GreenCurve = new PointCollection(green);
            BlueCurve = new PointCollection(blue);
	    }

	    private Point[] GetPoints(int[] values)
	    {
            int max = values.Max();
            Point[] points = new Point[values.Length + 2];
            points[0] = new Point(0, max);
            for (int i = 0; i < values.Length; i++)
            {
                points[i + 1] = new Point(i, max - values[i]);
            }
            points[values.Length + 1] = new Point(values.Length - 1, max);
	        return points;
	    }

        private static void OnImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var h = d as BitmapImageHistogram;
            if (h != null)
            {
                h.GetHistogram();
                h.image = (Bitmap)e.NewValue;
            }
        }
	}
}