// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using FirstFloor.ModernUI.Windows;
using System.Windows.Controls;
using Domen;
using System.Drawing;
using System;

namespace Client.Content
{
    /// <summary>
    /// Страница с графиками
    /// </summary>
    public partial class GraphicPage : UserControl, IContent
    {
        public GraphicPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Определяет график, необходимый к отображению
        /// </summary>
        /// <param name="e">An object that contains the navigation data.</param>
        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            try
            {
                graphicType = (GraphicType)int.Parse(e.Fragment);
                if (graphicType == GraphicType.Followers)
                    PrintGraphicFollowers();
                else if (graphicType == GraphicType.Following)
                    PrintGraphicFollowing();
                else PrintGraphicMedia();
            }
            catch
            {
               Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
            }

        }
        public void PrintGraphicFollowers()
        {
            Account account = ApiServer.GetAccount(long.Parse(Pages.MainPage.insta_id));

            Bitmap graph = Fomins.FominsFunctionality.GetGraph(account.GetFollowersArray(), account.GetTimeArray(),
                                                "Время", "Подписчики", "");

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = ApiServer.Bitmap2BitmapImage(graph);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            GraphicGrid.Children.Add(image);

        }
        public void PrintGraphicFollowing()
        {
            Account account = ApiServer.GetAccount(long.Parse(Pages.MainPage.insta_id));

            Bitmap graph = Fomins.FominsFunctionality.GetGraph(account.GetFollowingArray(), account.GetTimeArray(),
                                                "Время", "Подписки", "");
            //graph = new Bitmap("iamge.png");
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = ApiServer.Bitmap2BitmapImage(graph);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            GraphicGrid.Children.Add(image);

        }
        public void PrintGraphicMedia()
        {
            Account account = ApiServer.GetAccount(long.Parse(Pages.MainPage.insta_id));

            Bitmap graph = Fomins.FominsFunctionality.GetGraph(account.GetMediaArray(), account.GetTimeArray(),
                                                 "Время", "Медиа", "");
            //graph = new Bitmap("iamge.png");
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = ApiServer.Bitmap2BitmapImage(graph);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            GraphicGrid.Children.Add(image);

        }

        //Не реализованные методы
        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }
        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }
        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        GraphicType graphicType;
      
        enum GraphicType { Followers, Following, Media}
    }
}
