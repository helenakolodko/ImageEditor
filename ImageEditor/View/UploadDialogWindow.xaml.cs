using ImageEditor.Model;
using Nemiro.OAuth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ImageEditor.View
{
    /// <summary>
    /// Interaction logic for UploadDialogWindow.xaml
    /// </summary>
    public partial class UploadDialogWindow : FilesDialogWindow
    {
        public UploadDialogWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public EditableImage Image { get; set; }    

        public override string MethodName { get { return "Upload"; } }


        protected override void OnOpened(FileItem item)
        {
            var localPath = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
            Image.Source.Save(localPath, GetImageFormat(item.FileName));
            var destination = item.Path;
            Upload(localPath, destination);
        }

        private void Upload(string localPath, string destination)
        {
            var fs = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            OAuthUtility.PutAsync(
                "https://api-content.dropbox.com/1/files_put/auto/",
                new HttpParameterCollection
                { 
                    { "access_token" , Properties.Settings.Default.token },
                    { "overwrite", "true" },
                    { "autorename", "false" },
                    { "path", destination.Replace("\\", "/") },
                    { fs } 
                },
                callback: (RequestResult result) => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(() => UploadFile_Result(result)))
           );
        }

        private void UploadFile_Result(RequestResult result)
        {
            RequestEnd(result);
            UpdateList();
            DialogResult = true;
            Close();
        }

        private ImageFormat GetImageFormat(string imagePath)
        {
            ImageFormat result = ImageFormat.Png;
            string extention = Path.GetExtension(imagePath);
            if (extention == ".jpg")
            {
                result = ImageFormat.Jpeg;
            }
            else if (extention == ".bmp")
            {
                result = ImageFormat.Bmp;
            }
            else if (extention == ".gif")
            {
                result = ImageFormat.Gif;
            }
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                var localPath = Guid.NewGuid().ToString() + GetImageFormat(FileName); ;
                var destination = Path.Combine(string.IsNullOrWhiteSpace(CurrentFolder) ? "\\" : CurrentFolder,
                    Path.ChangeExtension(FileName, GetImageFormat(FileName).ToString()));
                Image.Source.Save(localPath, GetImageFormat(FileName));
                Upload(Image.Path, destination);
            }
        }
    }
}
