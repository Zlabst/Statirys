using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    /// <summary>
    /// Контроллер для генерации событий обновлений БД
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TimeController : ApiController
    {
        /// <summary>
        /// Получение информации о текущем таймере
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string TimerStatus()
        {
            string start = String.Format("Время инициализации таймера : {0}.", Models.TimerWork.startTime.ToString());
            string differ = String.Format("Время последнего обновления : {0}.", Models.TimerWork.lastTime);
            
            return start + Environment.NewLine + differ;
        }

        /// <summary>
        /// Ручное обновление данных в БД
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DbUpdate()
        {
            Models.TimerWork.Timer_Elapsed(null, null);
            return "OK Updated";
        }
    }
}
