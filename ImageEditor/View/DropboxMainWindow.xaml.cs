using ImageEditor.Annotations;
using ImageEditor.Command;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ImageEditor.View
{
    public partial class DropboxMainWindow : Window, INotifyPropertyChanged
    {
        private string username;
        private bool isAuthorized;

        public event RaiseCanChange LogInChanged;
        public string UserName
        {
            get { return username; }
            private set
            {
                username = value;
                OnPropertyChanged();
            }
        }
        public bool IsAuthorized
        {
            get { return isAuthorized; }
            private set
            {
                isAuthorized = value;
                OnPropertyChanged();
            }
        }
        public DropboxMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
        }

        private void OnLogInChange()
        {
            if (LogInChanged != null)
            {
                LogInChanged();
            }
        }
        public void LogIn()
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.token))
            {
                GetAccessToken();
            }
            else
            {
                CheckAccessToken();
            }
            
        }

        private void CheckAccessToken()
        {
            OAuthUtility.GetAsync
            (
                "https://api.dropbox.com/1/account/info",
                new HttpParameterCollection 
                { 
                    { "access_token", Properties.Settings.Default.token }
                },
                callback: (RequestResult result) => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(() => CheckAccessToken_Result(result))) 
            );
        }
        private void CheckAccessToken_Result(RequestResult result)
        {
            if (result.StatusCode == 200)
            {
                SetUserInfo();
            }
            if (result.StatusCode == 401)
            {
                GetAccessToken();
            }
            else
                RequestEnd(result);
            
        }

        private void GetAccessToken()
        {
            var login = new DropboxLogin("nz379qg3he6d01k", "cxpgctot3djx9ph");

            login.ShowDialog();

            if (login.IsSuccessfully)
            {
                Properties.Settings.Default.token = login.AccessToken.Value;
                Properties.Settings.Default.Save();
                SetUserInfo();
                OnLogInChange();
            }
        }

        private void SetUserInfo()
        {
            OAuthUtility.GetAsync
            (
                "https://api.dropbox.com/1/account/info",
                new HttpParameterCollection 
                { 
                    { "access_token", Properties.Settings.Default.token }
                },
                callback: (RequestResult result) => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(() => SetInfo_Result(result)))
            );
        }

        private void SetInfo_Result(RequestResult result)
        {
            if (result.StatusCode == 200)
            {
                Properties.Settings.Default.userName = result["display_name"].ToString();
                Properties.Settings.Default.Save();
                UserName = Properties.Settings.Default.userName;
                IsAuthorized = true;
                OnLogInChange();
            }
            else
                RequestEnd(result);
        }

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
        public void LogIn_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }


        public void LogOff(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.token = null;
            Properties.Settings.Default.Save();
            IsAuthorized = false;
            UserName = string.Empty;
            OnLogInChange();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            Hide();
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
