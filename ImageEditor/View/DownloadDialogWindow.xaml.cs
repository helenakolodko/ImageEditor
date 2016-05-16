using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageEditor.View
{
    public partial class DownloadDialogWindow : FilesDialogWindow
    {
        public DownloadDialogWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public override string MethodName { get { return "Load"; } }

        public void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        public void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InProgress = false;
            DialogResult = true;
        }      

        protected override void OnOpened(FileItem item)
        {
            InProgress = true;
            string filePath = Path.Combine(string.IsNullOrWhiteSpace(CurrentFolder) ? "\\": CurrentFolder , item.FileName);
            Uri uri = new Uri(String.Format("{0}?access_token={1}", 
                String.Format("https://api-content.dropbox.com/1/files/auto{0}", filePath), 
                Properties.Settings.Default.token)); 
            FileName = Guid.NewGuid().ToString()+ Path.GetExtension(item.FileName);
            Client.DownloadProgressChanged += DownloadProgressChanged;
            Client.DownloadFileCompleted += DownloadFileCompleted;
            Client.DownloadFileAsync(uri, FileName);
        }
    }
}
