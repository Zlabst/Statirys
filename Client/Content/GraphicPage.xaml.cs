// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using FirstFloor.ModernUI.Windows;
using System.Windows.Controls;
using Domen;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Client.Content
{
    /// <summary>
    /// Страница с графиками
    /// </summary>
    public partial class GraphicPage : UserControl, IContent
    {
        private BackgroundWorker backgroundWorker;

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
            graphicType = (GraphicType)int.Parse(e.Fragment);
            ProgressRing.IsActive = true;
            backgroundWorker = (BackgroundWorker)this.FindResource("BackgroundWorker");
            backgroundWorker.RunWorkerAsync(graphicType);
        }

        public System.Windows.Controls.Image PrintGraphic(List<int> y, List<DateTime> x, string xLabel, string yLabel)
        {
            Bitmap graph = Fomins.FominsFunctionality.GetGraph(y, x, xLabel, yLabel, "");
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = ApiServer.Bitmap2BitmapImage(graph);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            return image;
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

        enum GraphicType { Followers, Following, Media }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Account account = ApiServer.GetAccount(long.Parse(Pages.MainPage.insta_id));

                e.Result = account;

                
            }
            catch (Exception exc)
            {
                Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else
            {
                Account account = (Account)e.Result;
                System.Windows.Controls.Image graph;
                if (graphicType == GraphicType.Followers)
                    graph = PrintGraphic(account.GetFollowersArray(), account.GetTimeArray(), "Подписчики", "Время");
                else if (graphicType == GraphicType.Following)
                    graph = PrintGraphic(account.GetFollowingArray(), account.GetTimeArray(), "Подписки", "Время");
                else graph = PrintGraphic(account.GetMediaArray(), account.GetTimeArray(), "Медиа", "Время");

                GraphicGrid.Children.Add(graph);
                ProgressRing.IsActive = false;
            }
            }
    }
}
