using Runtasker.Logic.Enumerations;
using System;

namespace Runtasker.Logic.Workers.BaseWorkers
{
    //Класс Worker должен содержать в себе репзиторий
    public class BaseContextWorker : IDisposable
    {
        #region Fields
        public DisposingInternalObjectsSetting DisposeInternals = DisposingInternalObjectsSetting.Yes;
        #endregion

        #region Constructors
        public BaseContextWorker(MyDbContext context)
        {
            Construct(context);
        }

        protected virtual void Construct(MyDbContext context)
        {
            Context = context;
        }
        #endregion

        #region Properties

        protected MyDbContext Context { get; set; }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && DisposeInternals == DisposingInternalObjectsSetting.Yes)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    Context.Dispose();
                    Context = null;
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~BaseContextWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
