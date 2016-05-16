using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageEditor.View
{
    public partial class FileItem : UserControl
    {
        private static BitmapImage folder = Convert(Properties.Resources.Folder_Open_icon);
        private static BitmapImage file = Convert(Properties.Resources.File_Pictures_icon);
       
        private bool isFolder;
        public FileItem()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FileItem));
        public static readonly DependencyProperty FileSizeProperty = DependencyProperty.Register("FileSize", typeof(int), typeof(FileItem));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(FileItem));

        public FilesDialogWindow Owner { get; set; }
        public string Path { get; set; }
        public bool IsFolder
        {
            get { return isFolder; }
            set
            {
                isFolder = value;
                if (value)
                {
                    ImageSource = folder;
                }
                else
                    ImageSource = file;
            }
        }
        public BitmapImage ImageSource
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

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

        private static BitmapImage Convert(Bitmap bitmap)
        {
            BitmapImage image = default(BitmapImage);
            if (bitmap != null)
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
            }

            return image;
        }
    }
}
