using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Workers.Attachments;
using System.IO;

namespace Runtasker.Logic.Workers.Files
{
    /// <summary>
    /// Класс работающий с файлами и обладающий огромной ответственностью (является устаревшим)
    /// </summary>
    public class SuperFileWorker
    {
        #region Конструкторы
        public SuperFileWorker(IMyDbContext context)
        {
            RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            Context = context;
            CheckForDirectories();
            Customer = new CustomerFileMethods(RootDirectory);
            AttachmentWorker = new AttachmentWorkerBase(RootDirectory);
        }
        #endregion

        #region Поля
        public string RootDirectory { get; private set; }

        IMyDbContext Context { get; set; }

        public AttachmentWorkerBase AttachmentWorker { get; private set; }

        public CustomerFileMethods Customer { get; private set; }

        #endregion

        #region Подготовительные методы
        private void CheckForDirectories()
        {

            string[] directories = new string[]
            {
                $"{RootDirectory}/Temporary",
                $"{RootDirectory}/Attachments",
                $"{RootDirectory}/Attachments/Temp",
                $"{RootDirectory}/Orders"
            };
            
            foreach(string directory in directories)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            

            
        }
        #endregion


    }
}
