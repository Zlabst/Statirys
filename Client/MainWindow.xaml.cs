// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using FirstFloor.ModernUI.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Основное окно сервиса
    /// </summary>

    public partial class MainWindow : ModernWindow
    {
        internal string access_token;

        public MainWindow()
        {
            InitializeComponent();
        }
        
    }
}
