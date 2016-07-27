using Domen;
using Domen.DbWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Server.Models
{
    /// <summary>
    /// Класс, инкапсулирующий таймер обновления БД
    /// </summary>
    public static class TimerWork
    {
        public static Timer timer = new Timer(1000 * 60 * 60 * 24);//период - 24 часа
        public static DateTime lastTime;
        public static DateTime startTime;
        static TimerWork()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            startTime = DateTime.Now;
        }
        
        /// <summary>
        /// Вызывается таймером
        /// Собирает данные и обновляет записи всех юзеров в БД
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        public static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                lastTime = DateTime.Now;
                EFRepository repository = new EFRepository();
                Account[] accounts =
                             repository.Accounts.ToArray();//получение всех аккаунтов из БД
                    foreach (Account account in accounts)
                    {

                        string response = ApiServer.UsersSelf(account.AccessToken);
                        dynamic dyn = JsonConvert.DeserializeObject<dynamic>(response);
                        account.UpdateInformation((int)dyn.data.counts.followed_by,
                                                  (int)dyn.data.counts.follows,
                                                  0,
                                                  (int)dyn.data.counts.media);

                        repository.Save(account);
                    }
            }
            catch
            {

            }
        }
    }
}