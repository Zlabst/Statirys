// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Domen
{

    /// <summary>
    /// Инкапуслирует интерфейс для работы с сервером
    /// </summary>
    public static class ApiServer
    {
        private static string uriMyApi = "http://instaapi-ru.1gb.ru/api/account/";//uri сервера
        public static string ErrorMessageForClient//сообщения об отсутствии интернета. Используется на клиенте
        {
            get
            {
                List<string> answers = new List<string>();

                answers.Add("Видимо лесные эльфы украли ваше подключение к интернету :(\nПроверьте его, нажмите \"ОК\" и Statirys постарается решить эту проблему.");
                answers.Add("Что-то интернета нет, наверное оператор тоже ищет покемонов.\nПроверьте подключение, нажмите \"ОК\" и Statirys постарается решить эту проблему.");
                answers.Add("Когда отключается интернет ты можешь погрузить в свои мысли и серьёзно поду...\nА вот мы не можем без интернета :( Проверьте подключение, нажмите \"ОК\"\nи Statirys постарается решить эту проблему. ");
                return answers[new Random().Next(0, answers.Count)];
            }
        }
       
        //Работа с БД
        public static void PostAccount(Account account)
        {
            string obj = JsonConvert.SerializeObject(account);
            POST(uriMyApi, obj);
        }
        public static Account GetAccount(long instaId)
        {
            string response = GET(uriMyApi, "instaId="+instaId.ToString());
            dynamic temp = JsonConvert.DeserializeObject(response);

            if (temp != null)
            {

                Account result = new Account((string)temp.Followers,
                                              (string)temp.Following,
                                              (string)temp.Likes,
                                              (string)temp.Media,
                                              (long)temp.InstaId,
                                              (string)temp.Start);
                return result;
            }
            else return null;
        }
        
        //Endpoint'ы
        public static string UsersSelf(string accessToken)
        {
            string uri = "https://api.instagram.com/v1/users/self/";
            string data = "access_token=" + accessToken;
            return GET(uri, data);
        }
        public static string UsersSelfMediaRecent(string accessToken, int count, string maxId = "")
        {
            string uri = "https://api.instagram.com/v1/users/self/media/recent/";
            string data = "access_token=" + accessToken;
            data += "&count=" + count.ToString();
            data += maxId == "" ? "" : "&max_id=" + maxId;
            return GET(uri, data);
        }

        //HTTP методы
        public static string GET(string Url, string Data = "")
        {
            WebRequest req;
            if (Data != "")
                req = WebRequest.Create(Url + "?" + Data);
            else
                req = WebRequest.Create(Url);

            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }
        public static string POST(string Url, string Data)
        {
            try
            {
                WebRequest req = WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/json";
                byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
                req.ContentLength = sentData.Length;
                Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                WebResponse res = req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);

                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                string Out = String.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }
                return Out;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        return text;
                    }
                }
            }
        }


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
    }
}
