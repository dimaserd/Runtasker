using Runtasker.Logic.Workers.Attachments;
using System.IO;

namespace Runtasker.Logic.Workers.Files
{
    public class SuperFileWorker
    {
        #region Constructors
        public SuperFileWorker(MyDbContext context)
        {
            RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            Context = context;
            CheckForDirectories();
            Customer = new CustomerFileMethods(RootDirectory);
            AttachmentWorker = new AttachmentWorkerBase(RootDirectory);
        }
        #endregion

        #region Fields
        public string RootDirectory { get; private set; }

        MyDbContext Context { get; set; }

        public AttachmentWorkerBase AttachmentWorker { get; private set; }

        public CustomerFileMethods Customer { get; private set; }

        #endregion

        #region Preparation Methods
        //подготовительные методы
        private void CheckForDirectories()
        {
            if(!Directory.Exists($"{RootDirectory}/Temporary"))
            {
                Directory.CreateDirectory($"{RootDirectory}/Temporary");
            }

            if(!Directory.Exists($"{RootDirectory}/Attachments"))
            {
                Directory.CreateDirectory($"{RootDirectory}/Attachments");
            }

            if (!Directory.Exists($"{RootDirectory}/Attachments/Temp"))
            {
                Directory.CreateDirectory($"{RootDirectory}/Attachments/Temp");
            }

            if(!Directory.Exists($"{RootDirectory}/Orders"))
            {
                Directory.CreateDirectory($"{RootDirectory}/Orders");
            }
        }
        #endregion

        #region Public Methods
       
        #endregion


    }
}
