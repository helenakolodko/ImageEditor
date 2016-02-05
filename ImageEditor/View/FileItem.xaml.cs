using System.Windows;
using System.Windows.Controls;

namespace ImageEditor.View
{
    public partial class FileItem : UserControl
    {
        private string Format;

        public FileItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FileItem));
        public static readonly DependencyProperty FileSizeProperty = DependencyProperty.Register("FileSize", typeof(int), typeof(FileItem));

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public int FileSize
        {
            get { return (int)GetValue(FileSizeProperty); }
            set { SetValue(FileSizeProperty, value); }
        }
    }
}
