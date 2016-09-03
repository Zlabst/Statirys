// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"  

using Client.Pages;
using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.Content
{
    /// <summary>
    /// Interaction logic for StatPage.xaml
    /// </summary>
    public partial class StatPage : UserControl
    {
        private BackgroundWorker backgroundWorker;

        public StatPage()
        {
            InitializeComponent();
            backgroundWorker = (BackgroundWorker)this.FindResource("BackgroundWorker");

            //Необходимо для отображения прогресса загрузки
            ProgressRing.IsActive = true;
            MainGrid.Visibility = Visibility.Hidden;
            LoadingPanel.Visibility = Visibility.Visible;
            MainPage.ProgressRing.Visibility = Visibility.Visible;
            MainPage.ProgressPercentBlock.Visibility = Visibility.Visible;
            MainPage.ProgressPercentBlock.Text = "0%";

            backgroundWorker.RunWorkerAsync();

        }


        private void UserControl_Initialized(object sender, System.EventArgs e)
        {
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
           
            Action<int> updateProgress = new Action<int>((alreadyLoaded) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (MainPage.MediaCount != 0)
                    {
                        ProgressPercent.Text = Math.Round((((double)alreadyLoaded / MainPage.MediaCount) * 100), 0).ToString() + "%";
                        MainPage.ProgressPercentBlock.Text = ProgressPercent.Text;
                    }
                }));
            });
            var list = Domen.ApiServer.GetAllMedia(Pages.MainPage.access_token, updateProgress);
            e.Result = list;

        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Client.Pages.MainPage.ShowMessage(ApiServer.ErrorMessageForClient);
                return;
            }

            List<dynamic> result = (List<dynamic>)e.Result;

            int maxLikesFoto = -1;
            dynamic maxLikesFotoPost = "";

            int maxLikesVideo = -1;
            dynamic maxLikesVideoPost = "";

            int maxCommentsFoto = -1;
            dynamic maxCommentsFotoPost = "";

            int maxCommentsVideo = -1;
            dynamic maxCommentsVideoPost = "";

            int totalComments = 0;
            int totalCommentsFoto = 0;
            int totalCommentsVideo = 0;

            int totalLikes = 0;
            int totalLikesFoto = 0;
            int totalLikesVideo = 0;
            int totalFotos = 0;
            int totalVideos = 0;

            foreach (dynamic o in result)
            {
                totalLikes += (int)o.likes.count;
                totalComments += (int)o.comments.count;
                if ((string)o.type == "image")
                {
                    totalFotos += 1;
                    totalLikesFoto += (int)o.likes.count;
                    totalCommentsFoto += (int)o.comments.count;
                    if ((int)o.likes.count > maxLikesFoto)
                    {
                        maxLikesFoto = (int)o.likes.count;
                        maxLikesFotoPost = o;
                    }

                    if ((int)o.comments.count > maxCommentsFoto)
                    {
                        maxCommentsFoto = (int)o.comments.count;
                        maxCommentsFotoPost = o;
                    }

                }
                else
                {
                    totalLikesVideo += (int)o.likes.count;
                    totalVideos += 1;
                    totalCommentsVideo += (int)o.comments.count;
                    if ((int)o.likes.count > maxLikesVideo)
                    {
                        maxLikesVideo = (int)o.likes.count;
                        maxLikesVideoPost = o;
                    }

                    if ((int)o.comments.count > maxCommentsVideo)
                    {
                        maxCommentsVideo = (int)o.comments.count;
                        maxCommentsVideoPost = o;
                    }
                }
            }

            TotalLikes.Text = totalLikes.ToString();
            LikesPerMedia.Text = result.Count != 0 ? Math.Round(((double)totalLikes) / result.Count, 1).ToString() : "0";
            LikesPerFoto.Text = totalFotos != 0 ? Math.Round(((double)totalLikesFoto) / totalFotos, 1).ToString() : "0";
            LikesPerVideo.Text = totalVideos != 0 ? Math.Round(((double)totalLikesVideo) / totalVideos, 1).ToString() : "0";

            TotalComments.Text = totalComments.ToString();
            CommentsPerMedia.Text = result.Count != 0 ? Math.Round(((double)totalComments) / result.Count, 1).ToString() : "0";
            CommentsPerFoto.Text = totalFotos != 0 ? Math.Round(((double)totalCommentsFoto) / totalFotos, 1).ToString() : "0";
            CommentsPerVideo.Text = totalVideos != 0 ? Math.Round(((double)totalCommentsVideo) / totalVideos, 1).ToString() : "0";

            //Установка инфы на шапке приложения
            MainPage.TotalLikesBlock.Text = TotalComments.Text;
            MainPage.LikesPerMediaBlock.Text = LikesPerMedia.Text;
            MainPage.MostLikedMediaBlock.Text = Math.Max(maxLikesFoto, maxLikesVideo).ToString();

            if (maxLikesFoto != -1)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(250);
                PopularFoto.RowDefinitions.Add(row);

                int comments = (int)maxLikesFotoPost.comments.count;

                MediaPage.PutFoto((string)maxLikesFotoPost.images.standard_resolution.url, maxLikesFoto, comments, PopularFoto, 0, 0);
            }

            if (maxLikesVideo != -1)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(250);
                PopularVideo.RowDefinitions.Add(row);

                int comments = (int)maxLikesVideoPost.comments.count;

                MediaPage.PutFoto((string)maxLikesVideoPost.images.standard_resolution.url, maxLikesVideo, comments, PopularVideo, 0, 0);
            }

            if(maxCommentsFoto != -1)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(250);
                CommentPopularFoto.RowDefinitions.Add(row);

                int likes = (int)maxCommentsFotoPost.likes.count;
                MediaPage.PutFoto((string)maxCommentsFotoPost.images.standard_resolution.url, likes, maxCommentsFoto, CommentPopularFoto, 0, 0); 
            }

            if (maxCommentsVideo != -1)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(250);
                CommentPopularVideo.RowDefinitions.Add(row);

                int likes = (int)maxCommentsVideoPost.likes.count;
                MediaPage.PutFoto((string)maxCommentsVideoPost.images.standard_resolution.url, likes, maxCommentsVideo, CommentPopularVideo, 0, 0);
            }

            ProgressRing.IsActive = false;
            LoadingPanel.Visibility = Visibility.Hidden;
            MainPage.ProgressPercentBlock.Visibility = Visibility.Hidden;
            MainPage.ProgressRing.Visibility = Visibility.Hidden;

            MainGrid.Visibility = Visibility.Visible;
        }
    }
}
