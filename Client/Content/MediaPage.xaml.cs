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
                        response = ApiServer.UsersSelfMediaRecent(Pages.MainPage.access_token, 3);
                    else
                        response = ApiServer.GET(maxId);
                    dynamic data = JsonConvert.DeserializeObject(response);
                    
                    foreach (dynamic o in data.data)
                    {
                        int comments = (int)o.comments.count;
                        int likes = (int)o.likes.count;

                        PutFoto((string)o.images.standard_resolution.url, likes, comments, MediaGrid, nextGridX, nextGridY);

                        nextGridX = (nextGridX + 1) % 3;
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
        /// <summary>
        /// Вставляет фото в ячейку Grid'a и добавляет футер
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="likes">The likes.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="grid">The grid.</param>
        /// <param name="cellX">The cell column</param>
        /// <param name="cellY">The cell row</param>
        public static void PutFoto(string uri, int likes, int comments, Grid grid, int cellX, int cellY)
        {
            ModernProgressRing progress = new ModernProgressRing();
            progress.IsActive = true;
            Grid.SetRow(progress, cellY);
            Grid.SetColumn(progress, cellX);
            grid.Children.Add(progress);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri);
            bitmapImage.EndInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.None;

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            image.Margin = new Thickness(10);

            Grid.SetRow(image, cellY);
            Grid.SetColumn(image, cellX);

            grid.Children.Add(image);

            InfoTabForImages info = new InfoTabForImages();
            info.HorizontalAlignment = HorizontalAlignment.Stretch;
            info.Likes.Text = likes.ToString();
            info.Comments.Text = comments.ToString();
            

            Grid bottomTab = new Grid();
            bottomTab.Margin = new Thickness(10);
            bottomTab.HorizontalAlignment = HorizontalAlignment.Stretch;
            bottomTab.VerticalAlignment = VerticalAlignment.Bottom;
            bottomTab.Background = new SolidColorBrush(new System.Windows.Media.Color() { A = 130, B = 0, R = 0, G = 0 });

            bottomTab.Children.Add(info);

            Grid.SetRow(bottomTab, cellY);
            Grid.SetColumn(bottomTab, cellX);
            grid.Children.Add(bottomTab);

        }

        static string maxId = "";//используется для постраничной загрузки фото из инстаграмма

        //координаты следующей фото
        static int nextGridX = 0;
        static int nextGridY = 0;


    }

}
