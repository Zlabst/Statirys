// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using Domen;
using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client.Pages
{
    /// <summary>
    /// Основная страница
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Load();
        }

        public void Load()//отправляет юзера серверу
        {
            try
            {
                string response = ApiServer.UsersSelf(access_token);
                dynamic dyn = JsonConvert.DeserializeObject<dynamic>(response);
                Account account = new Account((int)dyn.data.counts.followed_by,
                                              (int)dyn.data.counts.follows, 0,
                                              (int)dyn.data.counts.media,
                                              (long)dyn.data.id)
                { AccessToken = access_token };
                insta_id = account.InstaId.ToString();

                if (ApiServer.GetAccount(account.InstaId)==null)
                {
                    ApiServer.PostAccount(account);
                }
            }
            catch
            {
                ShowMessage(ApiServer.ErrorMessageForClient);
                Load();
            }
        }
        private void UpdatePage()
        {
            string response = ApiServer.UsersSelf(access_token);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

            //Установка основной инфы
            UserName.Text = data.data.username;
            Followers.Text = data.data.counts.followed_by;
            Following.Text = data.data.counts.follows;
            Media.Text = data.data.counts.media;
            
            //Установка фотографии профиля
            BitmapImage profileFoto = new BitmapImage();
            profileFoto.BeginInit();
            profileFoto.UriSource = new Uri((string)data.data.profile_picture);
            profileFoto.EndInit();
            MainPhoto1.Source = profileFoto;
        }

        //Управление табом прогресса (пока не используются)
        internal void StartProgressing()
        {
            MainProgressBar.IsActive = true;
        }
        internal void StopProgressing()
        {
            MainProgressBar.IsActive = false;
        }
      
        public static void ShowMessage(string text)//"Прокачанный" MessageBox
        {
            var dlg = new ModernDialog
            {
                Title = "Statirys",
                Content = text
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CloseButton };
            dlg.CloseButton.Click += CloseButton_Click; ;
            dlg.ShowDialog();
        }

        //Обработчики событий
        private static void CloseButton_Click(object sender, RoutedEventArgs e)
        {
              Environment.Exit(0);            
        }
        private void Question_Click(object sender, RoutedEventArgs e)
        {
            string text = "Продукт произведён в России в 2016 году компанией IRYS.";
            var dlg = new ModernDialog
            {
                Title = "Statirys",
                Content = text
            };
            dlg.Buttons = new Button[] { dlg.OkButton };
            dlg.ShowDialog();

        }
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.Refresh.Execute("/Pages/MainPage.xaml", this);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StartProgressing();
                Client.Content.MediaPage.Reset();
                UpdatePage();
                StopProgressing();
            }
            catch
            {
                ShowMessage(ApiServer.ErrorMessageForClient);
                Load();
            }
        }

        internal static string access_token = "2242190593.8e4243a.644c8996283f481bb5d68239388824e7";//тестовый access_token (yurij_volkov)
        internal static string insta_id;//id юзера

    }
}
