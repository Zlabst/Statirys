// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using Client.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Client.Content
{
    /// <summary>
    /// Элемент управления - панель отображения данных внизу фото
    /// </summary>
    public partial class InfoTabForImages : UserControl
    {
        private string MediaId { get; set; }
        public InfoTabForImages(string mediaId)
        {
            InitializeComponent();
            if (mediaId == null)
                this.MouseLeftButtonUp -= appbar_message_MouseLeftButtonUp;
            MediaId = mediaId;

        }

        private void appbar_message_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show(Domen.ApiServer.MediaMediaId(MainPage.access_token, MediaId));
        }
    }
}
