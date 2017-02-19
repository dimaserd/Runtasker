using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Runtasker.Logic.Workers.Logging
{
    public class LoggingWorker : IDisposable
    {
        string _filesDir;

        protected string FilesDirectory
        {
            get
            {
                if(_filesDir == null)
                {
                    _filesDir = HostingEnvironment.MapPath("~/Files");
                }
                return _filesDir;
            }
            
        }

        #region Public Methods
        public void LogTextToFile(string fileName, string fileContents)
        {
            string filePath = $"{FilesDirectory}/{fileName}";

            StringBuilder sb = new StringBuilder();
            sb.Append($"{DateTime.Now}___________________________________________\n")
            .Append(fileContents)
            .Append("\n")
            .Append("___________________________________________\n");

            File.AppendAllText(filePath, sb.ToString());


        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
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
        // ~LoggingWorker() {
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
