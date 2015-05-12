using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ImageEditor.Annotations;
using AForge.Imaging;

namespace ImageEditor.View
{
	/// <summary>
	/// Interaction logic for BitmapImageHistogram.xaml
	/// </summary>
	public partial class BitmapImageHistogram:INotifyPropertyChanged
	{
        private System.Drawing.Image _image;
        public System.Drawing.Image Image
	    {
	        get { return _image; }
	        set
	        {
	            _image = value;
	            GetHistogram();
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
            ImageStatistics rgbStatistics = new ImageStatistics((Bitmap) Image);
	        RedCurve = GetPoints(rgbStatistics.Red.Values);
            GreenCurve = GetPoints(rgbStatistics.Green.Values);
            BlueCurve = GetPoints(rgbStatistics.Blue.Values);
	    }

	    private PointCollection GetPoints(int[] values)
	    {
            int max = values.Max();

            PointCollection points = new PointCollection();
            points.Add(new System.Windows.Point(0, max));
            for (int i = 0; i < values.Length; i++)
            {
                points.Add(new System.Windows.Point(i, max - values[i]));
            }
            points.Add(new System.Windows.Point(values.Length - 1, max));
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