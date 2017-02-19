
using Runtasker.Logic.Workers.Files;

namespace Runtasker.Logic.Workers.Developer
{
    public class DeveloperWorker
    {
        #region Constructors
        public DeveloperWorker()
        {
            Construct();
        }

        void Construct()
        {
            Context = new MyDbContext();
            FileWorker = new SuperFileWorker(Context);
            Logs = new DeveloperLogs();
            Reset = new DeveloperResetWorker(Context);
            Moq = new DeveloperMoqMethods(Context);
            DataBase = new DataBaseMethods(Context);
        }
        #endregion

        #region Private Properties
        static MyDbContext Context { get; set; }
        private SuperFileWorker FileWorker { get; set; }
        #endregion

        #region Public Properties
        public DataBaseMethods DataBase { get; private set; }
        public DeveloperLogs Logs { get; private set; }
        public DeveloperResetWorker Reset { get; private set; }
        public DeveloperMoqMethods Moq { get; private set; }
        #endregion
    }
}
