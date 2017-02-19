using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using VkParse.Statics;

namespace VkParser.Workers.Api
{
    //Класс у которого есть токен и самый главный метод обращения к апи
    public class VkApiWorkerBase : IDisposable
    {
        #region Constructors
        public VkApiWorkerBase()
        {
            Construct();
        }

        void Construct()
        {
            _tokenGiver = new TokenGiver();
        }
        #endregion

        #region Fields
        TokenGiver _tokenGiver;
        #endregion

        #region Protected Properties
        public string Token
        {
            get
            {
                return _tokenGiver.Token;
            }
        }
        #endregion

        #region Constants
        protected const ulong appID = 5335054;                      // ID приложения
        protected const string email = "+79099275162";         // email или телефон
        protected const string pass = "Fifa16muversusfcb";               // пароль для авторизации
        protected const long UserId = 399521094;
        #endregion
        
        #region Api calls
        public JObject Request(string method, string paramsString)
        {
            string resp = string.Empty;

            string url = @"https://api.vk.com/method/" + $"{method}"
                + $"?access_token={Token}&{paramsString}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resp = reader.ReadToEnd();
            }

            JObject json = JObject.Parse(resp);
            return json;
        }

        public async Task<JObject> RequestAsync(string method, string paramsString)
        {
            string resp = string.Empty;

            string url = @"https://api.vk.com/method/" + $"{method}"
                + $"?access_token={Token}&{paramsString}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resp = await reader.ReadToEndAsync();
            }

            JObject json = JObject.Parse(resp);
            return json;
        }


        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkApiWorkerBase() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
