using ImageEditor.Annotations;
using Nemiro.OAuth;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ImageEditor.View
{
    public partial class FilesDialogWindow : Window, INotifyPropertyChanged
    {
        private string currentFolder;
        private FileItem selected;
        private bool inProgress;
        private ObservableCollection<FileItem> items;
        private int progress;
        private WebClient Client = new WebClient();


        public event PropertyChangedEventHandler PropertyChanged;

        public FilesDialogWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

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
        public string MethodName { get { return "Load"; } }
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

        public FileItem SelectedItem { get { return selected; } set { selected = value; OnPropertyChanged(); } }
        public bool InProgress { get { return inProgress; } set { inProgress = value; OnPropertyChanged(); } }

        private void UpdateList()
        {
            throw new System.NotImplementedException();
        }

        

        private void RequestEnd(RequestResult result)
        {
            if (result.StatusCode < 200 || result.StatusCode > 299)
            {
                if (result["error"].HasValue)
                {
                    MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
