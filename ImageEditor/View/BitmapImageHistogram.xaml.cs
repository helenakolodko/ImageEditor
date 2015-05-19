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
	/// <summary>
	/// Interaction logic for BitmapImageHistogram.xaml
	/// </summary>
	public partial class BitmapImageHistogram : INotifyPropertyChanged
	{
        private Bitmap _image;
        public Bitmap Image
	    {
            get { return (Bitmap)GetValue(ImageProperty); }
	        set { SetValue(ImageProperty, value); }
	    }

        public static readonly DependencyProperty ImageProperty = 
            DependencyProperty.Register("Image", typeof(Bitmap), typeof(BitmapImageHistogram),
            new PropertyMetadata(default(Bitmap), OnImagePropertyChanged));

        private static void OnImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var h = d  as BitmapImageHistogram;
            if (h != null)
            {
                h.GetHistogram();
                h._image = (Bitmap) e.NewValue;
            }
        }

	    private PointCollection _rCurve;

	    public PointCollection RedCurve
	    {
	        get { return _rCurve; }
	        set
	        {
	            _rCurve = value;
	            OnPropertyChanged();
	        }
	    }

        private PointCollection _gCurve;

        public PointCollection GreenCurve
        {
            get { return _gCurve; }
            set
            {
                _gCurve = value;
                OnPropertyChanged();
            }
        }private PointCollection _bCurve;

        public PointCollection BlueCurve
        {
            get { return _bCurve; }
            set
            {
                _bCurve = value;
                OnPropertyChanged();
            }
        }

		public BitmapImageHistogram()
		{
			this.InitializeComponent();
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

	    public event PropertyChangedEventHandler PropertyChanged;

	    [NotifyPropertyChangedInvocator]
	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
	        var handler = PropertyChanged;
	        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
	    }
	}
}