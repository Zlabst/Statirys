using Client.Pages;
using Domen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Content
{
    /// <summary>
    /// Interaction logic for RelationsPage.xaml
    /// </summary>
    public partial class RelationsPage : UserControl
    {
        private BackgroundWorker backgroundWorker;
        public RelationsPage()
        {
            InitializeComponent();
            ProgressRing.IsActive = true;
            backgroundWorker = (BackgroundWorker)this.FindResource("BackgroundWorker");
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string response = ApiServer.UsersSelfFollows(MainPage.access_token); 
            dynamic data = JsonConvert.DeserializeObject(response);
            List<string> follows = new List<string>();
            foreach(dynamic o in data.data)
            {
                follows.Add((string)o.id);
            }
            e.Result = follows;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            List<string> follows = (List<string>)e.Result;
            MessageBox.Show(follows.Count.ToString());
        }
    }
}
