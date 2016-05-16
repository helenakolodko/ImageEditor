using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;

namespace ImageEditor.Model
{
    //public class WebBrowserCallbackEventArgs : EventArgs
    //{
    //    public Uri Url { get; set; }

    //    public WebBrowserCallbackEventArgs(Uri url)
    //    {
    //        this.Url = url;
    //    }
    //}

    //public delegate void WebBrowserCallback(object sender, WebBrowserCallbackEventArgs e);
    //class DropboxLogin
    //{
    //    private OAuthBase client;
    //    private string authorizationUrl;
    //    private bool canLogin;
    //    private bool canLogout;
    //    private bool autoLogout;
    //    private WebBrowserCallback callback = null;
    //    private bool accessTokenProcessing = false;
    //    private bool timeout = false;
    //    private string authorizationCode = "";
    //    private string AuthorizationCode
    //{
    //  get
    //  {
    //    return authorizationCode;
    //  }
    //  set
    //  {
    //    Debug.WriteLine(String.Format("Set AuthorizationCode {0}", value), "LoginForm");

    //    // bad solution...
    //    authorizationCode = value;
    //    this.canLogin = false;

    //    if (this.autoLogout && this.canLogout)
    //    {
    //      // clear browser cookies
    //      this.Logout();
    //    }
    //    else
    //    {
    //      // get access token by auth code
    //      this.GetAccessToken();
    //    }
    //  }
    //}
    //    public DropboxLogin ()
    //    {
    //        this.SetIEVersion();
    //        this.SetProgressImage(global::Nemiro.OAuth.LoginForms.Properties.Resources.loader2);
    //    }
    //    public DropboxLogin(OAuthBase client, bool autoLogout = false)
    //        : this()
    //    {
    //      this.Client = client;
    //      this.AutoLogout = autoLogout;
    //      this.Text = String.Format(this.Text, this.Client.ProviderName);
    //      this.CanLogin = true;
    //      this.CanLogout = true;

    //      this.webBrowser1.ScriptErrorsSuppressed = true;
    //      this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;

    //      this.Controls.SetChildIndex(this.webBrowser1, 1);
    //      this.Controls.SetChildIndex(this.pictureBox1, 0);

    //      this.AuthorizationUrl = this.Client.AuthorizationUrl;

    //      Thread t = null;

    //      if (this.AutoLogout)
    //      {
    //        t = new Thread(() => this.Logout());
    //      }
    //      else
    //      {
    //        t = new Thread(() => this.SetUrl(this.AuthorizationUrl));
    //      }

    //      t.IsBackground = true;
    //      t.Start();
    //    }
    //    public DropboxLogin(string clientId, string clientSecret, bool autoLogout = false) : this(clientId, clientSecret, null, autoLogout) { }
    //    public DropboxLogin(string clientId, string clientSecret, string scope, bool autoLogout = false) : this(new DropboxClient(clientId, clientSecret) { Scope = scope }, autoLogout) { }
    //    public DropboxLogin(DropboxClient client, bool autoLogout = false) : base(client, autoLogout) 
    //    {
    //      this.Width = 695;
    //      this.Height = 515;
    //      this.Icon = Properties.Resources.dropbox;
    //    }

    //    public bool IsSuccessfully { get; protected set; }
    //    public AccessToken AccessToken
    //    {
    //      get
    //      {
    //        if (!this.IsSuccessfully) { return null; }
    //        return (AccessToken)this.client.AccessToken;
    //      }
    //    }
    //    public string AccessTokenValue
    //    {
    //      get
    //      {
    //        if (!this.IsSuccessfully) { return null; }
    //        return this.AccessToken.Value;
    //      }
    //    }
    //    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    //    {
    //        Debug.WriteLine(String.Format("Document {0}", this.webBrowser1.ReadyState), "LoginForm");
    //        Debug.WriteLine(e.Url.ToString(), "LoginForm");

    //        if (!this.Timeout)
    //        {
    //            if (this.webBrowser1.ReadyState != WebBrowserReadyState.Complete && this.webBrowser1.ReadyState != WebBrowserReadyState.Interactive) // || this.webBrowser1.IsBusy
    //            {
    //                return;
    //            }
    //        }

    //        if (this.Callback != null)
    //        {
    //            Debug.WriteLine("Custom Callback", "LoginForm");
    //            this.Callback(sender, new WebBrowserCallbackEventArgs(e.Url));
    //        }
    //        else
    //        {
    //            this.DefaultCallback(sender, new WebBrowserCallbackEventArgs(e.Url));
    //        }
    //    }
    //    private void timer1_Tick(object sender, EventArgs e)
    //    {
    //      Debug.WriteLine("Timeout", "LoginForm");
    //      this.Timeout = true;
    //      this.Enabled = false;
    //      this.webBrowser1_DocumentCompleted(this.webBrowser1, new WebBrowserDocumentCompletedEventArgs(this.webBrowser1.Url));
    //    }

    //    private void DefaultCallback(object sender, WebBrowserCallbackEventArgs e)
    //    {
    //      Debug.WriteLine("Default Callback", "LoginForm");

    //      // waiting for results
    //      if (e.Url.Query.IndexOf("code=") != -1 || e.Url.Query.IndexOf("oauth_verifier=") != -1)
    //      {
    //        this.CanLogin = false;

    //        // is result
    //        var v = UniValue.ParseParameters(e.Url.Query.Substring(1));

    //        if (v.ContainsKey("code"))
    //        {
    //          this.AuthorizationCode = v["code"].ToString();
    //        }
    //        else
    //        {
    //          this.AuthorizationCode = v["oauth_verifier"].ToString();
    //        }

    //        return;
    //      }

    //      // access denied
    //      if (!String.IsNullOrEmpty(e.Url.Query) && e.Url.Query.IndexOf("error=access_denied") != -1)
    //      {
    //        this.Close();
    //      }

    //      // hide progress
    //      if (!this.AccessTokenProcessing) // is impossible to determine the exact address
    //      {
    //        this.HideProgress();
    //      }

    //      // additional custom handler
    //      if (typeof(ILoginForm).IsAssignableFrom(this.GetType()))
    //      {
    //        this.CanLogin = false;
    //        Debug.WriteLine("ILoginForm", "LoginForm");
    //        ((ILoginForm)this).WebDocumentLoaded(this.webBrowser1, e.Url);
    //      }
    //    }

    //    private void GetAccessToken(string authorizationCode = "")
    //    {
    //      if (this.AccessTokenProcessing) { return; }

    //      if (String.IsNullOrEmpty(authorizationCode))
    //      {
    //        authorizationCode = this.AuthorizationCode;
    //      }

    //      this.AccessTokenProcessing = true;
    //      this.SetProgressImage(global::Nemiro.OAuth.LoginForms.Properties.Resources.loader);
    //      this.ShowProgress();

    //      var t = new Thread(GetAccessTokenThread);
    //      t.IsBackground = true;
    //      t.Start(authorizationCode);
    //    }

    //    private void GetAccessTokenThread(object args)
    //    {
    //      Debug.WriteLine(String.Format("GetAccessTokenThread {0}", args), "LoginForm");

    //      // verify code
    //      this.Client.AuthorizationCode = args.ToString();

    //      try
    //      {
    //        this.IsSuccessfully = this.Client.AccessToken.IsSuccessfully;
    //      }
    //      catch (Exception ex)
    //      {
    //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //      }

    //      // this.AccessTokenProcessing = false;
    //      this.Close();
    //    }
    //    private void ShowProgress()
    //    {
    //      if (this.InvokeRequired)
    //      {
    //        this.Invoke(new Action(ShowProgress));
    //        return;
    //      }

    //      Debug.WriteLine("ShowProgress", "LoginForm");

    //      // because document is not loaded
    //      //this.webBrowser1.Visible = false;
    //      this.pictureBox1.Visible = true;
    //    }
    //    private void HideProgress()
    //    {
    //      if (this.InvokeRequired)
    //      {
    //        this.Invoke(new Action(HideProgress));
    //        return;
    //      }

    //      Debug.WriteLine("HideProgress", "LoginForm");

    //      //this.webBrowser1.Visible = true;
    //      this.pictureBox1.Visible = false;
    //    }

    //    public void SetProgressImage(Image image)
    //    {
    //      this.pictureBox1.Image = image;
    //    }

    //    public new void Close()
    //    {
    //        if (this.InvokeRequired)
    //        {
    //        this.Invoke(new Action(Close));
    //        return;
    //        }
    //        // set dialog result
    //        if (this.IsSuccessfully)
    //        {
    //        this.DialogResult = System.Windows.Forms.DialogResult.OK;
    //        }
    //        else
    //        {
    //        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
    //        }
    //        // close form
    //        base.Close();
    //    }

    //    private void SetUrl(string url, WebBrowserCallback callback = null)
    //    {
    //      if (this.InvokeRequired)
    //      {
    //        this.Invoke(new Action<string, WebBrowserCallback>(SetUrl), url, callback);
    //        return;
    //      }

    //      Debug.WriteLine(String.Format("SetUrl {0}", url), "LoginForm");

    //      this.Timeout = false;
    //      this.timer1.Enabled = false;

    //      this.ShowProgress();
    //      this.Callback = callback;
    //      this.webBrowser1.Navigate(url);
    //    }

    //    private void StartWaiting()
    //    {
    //      if (this.InvokeRequired)
    //      {
    //        this.Invoke(new Action(StartWaiting));
    //        return;
    //      }

    //      this.timer1.Enabled = true;
    //    }

    //    private void SetIEVersion()
    //    {
    //      try
    //      {
    //        var programName = Path.GetFileName(Environment.GetCommandLineArgs().First());

    //        // get current version of IE emulation
    //        var currentEmulationVersion = BrowserEmulationVersion.Default;
    //        var emulationKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

    //        if (emulationKey != null)
    //        {
    //          object value = emulationKey.GetValue(programName, null);

    //          if (value != null)
    //          {
    //            try
    //            {
    //              currentEmulationVersion = (BrowserEmulationVersion)Enum.Parse(typeof(BrowserEmulationVersion), value.ToString().Split('.').First());
    //            }
    //            catch { }
    //          }
    //        }

    //        // get current IE version
    //        int ieVersion = 0;
    //        var ieKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer");

    //        if (ieKey != null)
    //        {
    //          object value = ieKey.GetValue("svcVersion", null) ?? ieKey.GetValue("Version", null);

    //          if (value != null)
    //          {
    //            int.TryParse(value.ToString().Split('.').First(), out ieVersion);
    //          }
    //        }

    //        // check versions
    //        if (ieVersion > 0 && currentEmulationVersion == BrowserEmulationVersion.Default)
    //        {
    //          // set IE emulation version
    //          if (ieVersion >= 11)
    //          {
    //            emulationKey.SetValue(programName, (int)BrowserEmulationVersion.IE11Edge, RegistryValueKind.DWord);
    //          }
    //          else
    //          {
    //            var v = BrowserEmulationVersion.IE7;
    //            switch (ieVersion)
    //            {
    //              case 10:
    //                v = BrowserEmulationVersion.IE10;
    //                break;

    //              case 9:
    //                v = BrowserEmulationVersion.IE9;
    //                break;

    //              case 8:
    //                v = BrowserEmulationVersion.IE8;
    //                break;
    //            }
    //            emulationKey.SetValue(programName, (int)v, RegistryValueKind.DWord);
    //          }
    //        }
    //      }
    //      /*catch (SecurityException)
    //      {
    //      }
    //      catch (UnauthorizedAccessException)
    //      {
    //      }*/
    //      catch { }
    //    }

    //    protected internal void KillCookies()
    //    {
    //      if (this.webBrowser1.Document != null && !String.IsNullOrEmpty(this.webBrowser1.Document.Cookie))
    //      {
    //        var cookies = webBrowser1.Document.Cookie.Split(';').ToList();
    //        foreach (var c in cookies)
    //        {
    //          var domains = this.webBrowser1.Url.Host.Split('.').ToList();
    //          while (domains.Count > 1)
    //          {
    //            this.webBrowser1.Document.Cookie = String.Format("{0}=; Thu, 01-Jan-1970 00:00:01 GMT; domain={1};", c.Split('=').First().Trim(), String.Join(".", domains));
    //            // path=
    //            domains.RemoveAt(0);
    //          }
    //        }
    //      }
    //    }
    //    public virtual void Logout()
    //    {
    //      Debug.WriteLine("Logout", "LoginForm");

    //      // goto home page
    //      var u = new Uri(this.AuthorizationUrl);

    //      this.SetUrl
    //      (
    //        String.Format("{0}://{1}", u.Scheme, u.Host),
    //        (object sender, WebBrowserCallbackEventArgs e) =>
    //        {
    //          // remove cookies
    //          this.KillCookies();
    //          // next action
    //          if (this.CanLogin)
    //          {
    //            // goto login
    //            this.SetUrl(this.AuthorizationUrl);
    //          }
    //          else
    //          {
    //            // can not login, get access token
    //            this.GetAccessToken();
    //          }
    //        }
    //      );
    //    }

    //    public void WebDocumentLoaded(System.Windows.Forms.WebBrowser webBrowser, Uri url)
    //    {
    //      // waiting for results
    //      if (url.ToString().Equals("about:blank", StringComparison.OrdinalIgnoreCase))
    //      {
    //        // the user has refused to give permission 
    //        this.Close();
    //      }
    //      else
    //      {
    //        if (webBrowser.Document.GetElementById("auth-code-input") != null)
    //        {
    //          // set authorization code
    //          base.AuthorizationCode = webBrowser.Document.GetElementById("auth-code-input").GetAttribute("value");
    //        }
    //      }
    //    }

    //}
}
