using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Runtasker.Logic.Workers.Files
{
    public class FileWorkerBase
    {
        #region Constructors
        public FileWorkerBase()
        {
            Construct(null);
        }


        public FileWorkerBase(string rootDirectory)
        {
            Construct(rootDirectory);
        }

        void Construct(string rootDirectory)
        {
            RootDirectory = rootDirectory ?? System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            CheckForDirectories();
        }

        void CheckForDirectories()
        {
            string[] directories = new string[] 
            {
                OrdersDirectory,
                ZipTempDirectory,
                AttachmentsDirectory
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

        #region Properties
        protected  string RootDirectory { get; set; }

        #region Helping Directories
        protected string OrdersDirectory { get { return $"{RootDirectory}/Orders"; } }
        protected string ZipTempDirectory { get { return $"{RootDirectory}/Zip/Temp"; } }
        protected string AttachmentsDirectory { get { return $"{RootDirectory}/Attachments"; } }
        #endregion

        #endregion

        #region Helping Methods
        protected string GetOrderDirectoryPath(int orderId)
        {
            return $"{OrdersDirectory}/{orderId}";
        }

        //Method creates zip with files in directory parameter
        //or throws exceptions if something is wrong
        protected string WriteZipWithAllFilesInFolderToAttachmentsDirectory(string directory)
        {
            if(!Directory.Exists(directory))
            {
                throw new Exception("Директории не существует!");
            }

            List<string> filenames = Directory.GetFiles(directory).ToList();

            if(filenames.Count() == 0)
            {
                return null;
            }

            string uniqueZipName = $"{Guid.NewGuid()}.zip";
            string zipPath = $"{AttachmentsDirectory}/{uniqueZipName}";

            ZipFile.CreateFromDirectory(directory, zipPath);

            return zipPath;
        }
        #endregion
    }
}
