using ImageEditor.Annotations;
using Nemiro.OAuth;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Controls;

namespace ImageEditor.View
{
    public abstract class FilesDialogWindow : Window, INotifyPropertyChanged
    {
        private string currentFolder = string.Empty;
        private FileItem selected;
        private bool inProgress;
        private ObservableCollection<FileItem> items;
        private int progress;

        protected WebClient Client = new WebClient();

        public event PropertyChangedEventHandler PropertyChanged;

        static FilesDialogWindow()
        {
            EventManager.RegisterClassHandler(typeof(FileItem), FileItem.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnItemClick));
            EventManager.RegisterClassHandler(typeof(FileItem), FileItem.MouseDoubleClickEvent,
                new MouseButtonEventHandler(OnItemDoubleClick));
        }
        private string fileName;
        public string FileName { get { return fileName; } set { fileName = value; OnPropertyChanged(); } }
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FileItem> Items
        {
            get
            {
                if (items == null)
                    items = new ObservableCollection<FileItem>();
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }
        public abstract string MethodName { get; }
        public string CurrentFolder
        {
            get { return currentFolder; }
            set
            {
                currentFolder = value;
                UpdateList();
                OnPropertyChanged();
            }
        }

        public FileItem SelectedItem { get { return selected; } set { selected = value; OnPropertyChanged();FileName = value.FileName; } }
        public bool InProgress { get { return inProgress; } set { inProgress = value; OnPropertyChanged(); } }

        protected void UpdateList()
        {
            OAuthUtility.GetAsync
                (
                    "https://api.dropbox.com/1/metadata/auto/",
                    new HttpParameterCollection 
                    { 
                        { "path", CurrentFolder.Replace("\\", "/") },
                        { "access_token", Properties.Settings.Default.token }
                    },
                    callback: (RequestResult result) => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(() => UpdateList_Result(result)))
                );
        }

        private void UpdateList_Result(RequestResult result)
        {
            var files = new ObservableCollection<FileItem>();

            if (result.StatusCode == 200)
            {
                var fileList = result["contents"]
                    .OrderBy(itm => Path.GetFileName(itm["path"].ToString()))
                    .OrderByDescending(itm => bool.Parse(itm["is_dir"].ToString()));
                foreach (UniValue file in fileList)
                {
                    var item = new FileItem() { Owner = this};
                    item.Path = file["path"].ToString();
                    item.FileName = Path.GetFileName(item.Path);
                    item.FileSize = int.Parse(file["bytes"].ToString());
                    item.IsFolder = bool.Parse(file["is_dir"].ToString());
                    files.Add(item);
                }

                if (!string.IsNullOrEmpty(CurrentFolder) && CurrentFolder != "\\")
                {
                    var root = new FileItem(){Owner = this};
                    root.Path = Path.GetDirectoryName(CurrentFolder);
                    root.FileName = "..";
                    root.IsFolder = true;
                    files.Insert(0, root);
                }
            }
            Items = files;
            this.RequestEnd(result);
        }

        public virtual bool? OpenDialog()
        {
            CurrentFolder = "\\";
            return ShowDialog();
        }

        public static void OnItemClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as FileItem;
            if (item != null)
            {
                item.Owner.OnSelected(item);
            }
        }

        public static void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as FileItem;
            if (item != null)
            {
                if (item.IsFolder)
                    item.Owner.CurrentFolder = item.Path;
                else
                    item.Owner.OnOpened(item);
            }
        }

        private void OpenFolder(FileItem item)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSelected(FileItem item)
        {
            SelectedItem = item;
        }
        protected abstract void OnOpened(FileItem item);

        protected void RequestEnd(RequestResult result)
        {
            if (result.StatusCode < 200 || result.StatusCode > 299)
            {
                if (result["error"].HasValue)
                {
                    MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Unknown Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel == true)
            {
                this.Client.CancelAsync();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
