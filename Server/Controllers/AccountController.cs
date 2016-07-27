// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Domen.DbWork;
using Domen;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Timers;
namespace Server.Controllers
{
    /// <summary>
    /// Главный контроллер сервера
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AccountController : ApiController
    {
        private EFRepository repository = new EFRepository();
      
        [HttpPost]
        public string SaveAccount([FromBody] Account account)
        {
            repository.Save(account);
            return "OK";
        }
        [HttpGet]
        public Account GetAccount(long instaId)
        {
            Account account = repository.Accounts.FirstOrDefault(x => x.InstaId == instaId);

            return account;
        }

        /// <summary>
        /// Зарегистрированный адрес редиректа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Redirect()
        {
            return "OK Redirected";
        }
        
        //Http методы
        private static string POST(string Url, string Data)
        {
            try
            {
                WebRequest req = WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
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
        private static string GET(string Url, string Data)
        {
            WebRequest req = WebRequest.Create(Url + "?" + Data);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

    }
}
