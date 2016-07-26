// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using System;
using System.Collections.Generic;
namespace Domen
{

    /// <summary>
    /// Инкапсулирует все данные о юзере
    /// </summary>
    public class Account
    {
        public Account()
        { }
        public Account(int startFollowers, int startFollowing,
                       int startLikes, int startMedia, long instaId)
        {
            Followers = startFollowers.ToString();
            Following = startFollowing.ToString();
            Likes = startLikes.ToString();
            InstaId = instaId;
            Media = startMedia.ToString();
            Start = DateTime.Now.ToString();
        }
        public Account(string Followers, string Following, 
                       string Likes, string Media, long InstaId, string Start)
        {
            this.Followers = Followers;
            this.Following = Following;
            this.Likes = Likes;
            this.Media = Media;
            this.InstaId = InstaId;
            this.Start = Start;
        }

        public string Start{ get; set; }
        public string Followers { get; set; }
        public string Following { get; set; }
        public string Media { get; set; }
        public string Likes { get; set; }
        public int Id { get; set; } //Id'шник записи в БД
        public long InstaId { get; set; } //Id'шник юзера
        public string AccessToken { get; set; }

        public List<int> GetFollowersArray()
        {
            List<int> result = new List<int>();
            string[] temp = Followers.Split('|');
            foreach (string s in temp)
                result.Add(int.Parse(s));
            return result;
        }
        public List<int> GetFollowingArray()
        {
            List<int> result = new List<int>();
            string[] temp = Following.Split('|');
            foreach (string s in temp)
                result.Add(int.Parse(s));
            return result;
        }
        public List<int> GetMediaArray()
        {
            List<int> result = new List<int>();
            string[] temp = Media.Split('|');
            foreach (string s in temp)
                result.Add(int.Parse(s));
            return result;
        }
        public List<int> GetLikesArray()
        {
            List<int> result = new List<int>();
            string[] temp = Likes.Split('|');
            foreach (string s in temp)
                result.Add(int.Parse(s));
            return result;
        }
        public List<DateTime> GetTimeArray()
        {
            List<DateTime> result = new List<DateTime>();
            string[] temp = Start.Split('|');
            foreach (string s in temp)
                result.Add(DateTime.Parse(s));
            return result;
        }

        public void UpdateInformation(int followers, int following, int likes, int media)
        {
            Followers += "|" + followers.ToString();
            Following += "|" + following.ToString();
            Media += "|" + media.ToString();
            Likes += "|" + likes.ToString();
            Start += "|" + DateTime.Now.ToString();
        }
        public override string ToString()
        {
            string s = string.Format("Подписчиков : {0}\nПодписок : {1}\nМедиа : {2}\nЛайков : {3}\nВремя : {4}",
                                  Followers,
                                  Following,
                                  Media,
                                  Likes,
                                  Start);
            return s;
        }
        
    }
}
