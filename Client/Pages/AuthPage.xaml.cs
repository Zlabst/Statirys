// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using Awesomium.Core;
using Domen;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Client
{
    /// <summary>
    /// Страница авторизации
    /// </summary>
    public partial class AuthPage : UserControl
    {
        public AuthPage()
        {
            InitializeComponent();
            Start();
        }

        /// <summary>
        /// Настраивает работу AuthWebBrowser'a
        /// Работает до начала процесса авторизации
        /// </summary>
        public void Start()
        {
            if (!CheckForInternetConnection())
            {
               Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
                Start();
            }
            else
            {
                WebSession session = WebCore.CreateWebSession(WebPreferences.Default);
                AuthWebBrowser.WebSession = session;
                try
                {
                    LogIn();
                }
                catch
                {
                   Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
                    LogIn();
                }
            }
        }

        /// <summary>
        /// Редиректит на страницу авторизации
        /// </summary>
        private void LogIn()
        {
            StartProgressing();

            string uriToGetCode = "https://api.instagram.com/oauth/authorize/?" +
                              "client_id=" + client_id +
                              "&redirect_uri=" + redirect_uri +
                              "&response_type=code" +
                              "&scope=public_content+follower_list";

            AuthWebBrowser.Source = new Uri(uriToGetCode);
        }

        /// <summary>
        /// //получает параметр parameter из запроса url
        /// </summary>
        /// <param name="url">The URI.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private string UriParser(string url, string parameter)
        {
            foreach (string uri in url.Split('?'))
            {
                foreach (string s in uri.Split('&'))
                {
                    string[] array = s.Split('=');
                    if (array[0] == parameter)
                        return array[1];
                }
            }
            return "";
        }

        private void StartProgressing()
        {
            AuthWebBrowser.Visibility = Visibility.Hidden;

        }
        private void StopProgressing()
        {
            AuthWebBrowser.Visibility = Visibility.Visible;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void AuthWebBrowser_DocumentReady(object sender, Awesomium.Core.DocumentReadyEventArgs e)
        {
            AuthWebBrowser.ExecuteJavascript("$('input[name=\"username\"]').blur();");
            StopProgressing();
        }
        private void AuthWebBrowser_AddressChanged(object sender, Awesomium.Core.UrlEventArgs e)
        {
            string code = UriParser(AuthWebBrowser.Source.AbsoluteUri, "code");
            string error = UriParser(AuthWebBrowser.Source.AbsoluteUri, "error");
            AuthWebBrowser.Focusable = true;

            if (code != "")
            {
                StartProgressing();

                //Getting access_token
                string url = @"https://api.instagram.com/oauth/access_token";   //Access_token
                string data = "client_id=" + client_id + "&client_secret=" + client_secret
                + "&grant_type=authorization_code" + "&redirect_uri=" + redirect_uri
                + "&code=" + code + "&response_type=token";

                var response = ApiServer.POST(url, data);
                dynamic din = JsonConvert.DeserializeObject<dynamic>(response);
                string accessToken = din.access_token;


                //перейти к основной странице
                Pages.MainPage.access_token = accessToken;
                NavigationCommands.GoToPage.Execute("/Pages/MainPage.xaml", this);

                return;
            }
            else if (error != "")
            {
                MessageBox.Show(error);
                LogIn();
            }
       
        }
        
        private string client_id = "8e4243a255074a1bbb6496bcc438be3b";
        private string client_secret = "f8fe4158192642b683045f6a5360d83a";
        private string redirect_uri = "http://instaapi-ru.1gb.ru/api/account/";
        
    }
}
