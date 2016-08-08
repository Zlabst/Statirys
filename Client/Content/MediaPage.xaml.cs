// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using Domen;
using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace Client.Content
{
    /// <summary>
    /// Страница - таблица фоток
    /// </summary>
    public partial class MediaPage : UserControl
    {
        public MediaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Сбрасывает данные коллекции фоток
        /// </summary>
        public static void Reset()
        {
            maxId = "";
            nextGridX = 0;
            nextGridY = 0;
        }
        private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (maxId != null)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(100);
                    MediaGrid.RowDefinitions.Add(row);
                    double d = MediaGrid.Height;

                    string response;
                    if (maxId == "")
                        response = ApiServer.UsersSelfMediaRecent(Pages.MainPage.access_token, 3, maxId);
                    else
                        response = ApiServer.GET(maxId);
                    dynamic data = JsonConvert.DeserializeObject(response);

                    foreach (dynamic o in data.data)
                    {

                        ModernProgressRing progress = new ModernProgressRing();
                        progress.IsActive = true;
                        Grid.SetRow(progress, nextGridY);
                        Grid.SetColumn(progress, (nextGridX) % 3);
                        MediaGrid.Children.Add(progress);

                        int comments = (int)o.comments.count;
                        int likes = (int)o.likes.count;

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri((string)o.images.standard_resolution.url);
                        bitmapImage.EndInit();
                        bitmapImage.CreateOptions = BitmapCreateOptions.None;

                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Source = bitmapImage;
                        image.Margin = new Thickness(10);

                        Grid.SetRow(image, nextGridY);
                        Grid.SetColumn(image, (nextGridX++) % 3);

                        MediaGrid.Children.Add(image);

                        InfoTabForImages info = new InfoTabForImages();
                        info.HorizontalAlignment = HorizontalAlignment.Stretch;
                        info.Likes.Text = likes.ToString();
                        info.Comments.Text = comments.ToString();

                        Grid bottomTab = new Grid();
                        bottomTab.Margin = new Thickness(10);
                        bottomTab.HorizontalAlignment = HorizontalAlignment.Stretch;
                        bottomTab.VerticalAlignment = VerticalAlignment.Bottom;
                        bottomTab.Background = new SolidColorBrush(new System.Windows.Media.Color() { A = 130, B = 0, R = 0, G = 0 });

                        double cellHeight = MediaGrid.RowDefinitions.First().Height.Value;
                        double cellWidth = MediaGrid.ColumnDefinitions.First().Width.Value;

                        bottomTab.Children.Add(info);

                        Grid.SetRow(bottomTab, nextGridY);
                        Grid.SetColumn(bottomTab, (nextGridX - 1) % 3);
                        MediaGrid.Children.Add(bottomTab);

                    }
                    maxId = data.pagination.next_url;
                    nextGridY++;


                    MainScrollViewer.ScrollToEnd();
                }
                if (maxId == null)
                {
                    LoadMoreButton.IsEnabled = false;
                }
            }
            catch
            {
                LoadMoreButton.IsEnabled = false;
                Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
                Reset();
            }

        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            string response = ApiServer.UsersSelf(Client.Pages.MainPage.access_token);
            dynamic dyn = JsonConvert.DeserializeObject<dynamic>(response);
            UserName.Text = (string)dyn.data.username;
            FullName.Text = (string)dyn.data.full_name;
            Bio.Text = (string)dyn.data.bio;
            LoadMoreButton_Click(sender, null); //запускает загрузку первых 3х фото
        }
        /// <summary>
        ///  Масштабирует размер фото
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void MediaGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            double width = MediaGrid.ColumnDefinitions.First().ActualWidth;

            foreach (var v in MediaGrid.RowDefinitions)
            {
                v.Height = new GridLength(width);
            }



        }

        static string maxId = "";//используется для постраничной загрузки фото из инстаграмма

        //координаты следующей фото
        static int nextGridX = 0;
        static int nextGridY = 0;

       
    }

}
