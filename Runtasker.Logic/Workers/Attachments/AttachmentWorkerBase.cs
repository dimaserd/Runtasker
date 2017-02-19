using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Logic.Extensions.Namers;

namespace Runtasker.Logic.Workers.Attachments
{
    public class AttachmentWorkerBase
    {
        #region Constructors
        public AttachmentWorkerBase()
        {
            Construct();
        }

        public AttachmentWorkerBase(string rootDirectory)
        {
            Construct(rootDirectory);
        }

        private void Construct(string rootDirectory = null)
        {
            _rootDirectory = rootDirectory ?? System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            CheckDirectories();
            Namer = new AttachmentNamer();
        }
        #endregion

        #region Preparation Methods
        private void CheckDirectories()
        {
            string[] directories = new string[]
            {
                ZipTempDirectory, AttachmentsDirectory, TempDirectory, OrdersDirectory
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

        #region Private Fields
        private string _rootDirectory;
        #endregion

        #region Protected Fields
        //Private fields with paths to directories
        protected string RootDirectory { get { return _rootDirectory; } }
        protected string TempDirectory { get { return $"{RootDirectory}/Temporary"; } }
        protected string OrdersDirectory { get { return $"{RootDirectory}/Orders"; } }
        protected string ZipTempDirectory { get { return $"{RootDirectory}/Zip/Temp"; } }
        protected string AttachmentsDirectory { get { return $"{RootDirectory}/Attachments"; } }
        
        //Gives Names for attachments
        protected AttachmentNamer Namer { get; set; }
        #endregion

        #region Public Methods

        public string GetToMessage(string attachments = "")
        {
            using (MyDbContext context = new MyDbContext())
            {
                if (attachments == null)
                {
                    return "";
                }
                List<string> filenames = attachments.Split('&').ToList();
                if (filenames.Count() == 0)
                {
                    return "";
                }

                if (filenames.Count() == 1)
                {
                    if (File.Exists($"{TempDirectory}/{filenames[0]}"))
                    {
                        string newFileName = $"{Guid.NewGuid()}.{filenames[0].Split('.').Last()}";
                        File.Move($"{TempDirectory}/{filenames[0]}", $"{AttachmentsDirectory}/{newFileName}");
                        Attachment a = new Attachment
                        {
                            Id = $"{Guid.NewGuid()}",
                            FileName = filenames[0],
                            FilePath = $"{AttachmentsDirectory}/{newFileName}"
                        };
                        context.Attachments.Add(a);
                        context.SaveChanges();
                        return $"/File/DownloadByKey?key={a.Id}";
                    }
                    else
                    {
                        return "";
                    }
                }

                if (filenames.Count() > 1)
                {
                    new DirectoryInfo(ZipTempDirectory).Clear(); //очистили директорию для временки

                    foreach (string filename in filenames)
                    {
                        if (File.Exists($"{TempDirectory}/{filename}"))
                        {
                            File.Move($"{TempDirectory}/{filename}", $"{ZipTempDirectory}/{filename}");
                        }
                    }
                    string zipName = string.Format($"{Guid.NewGuid()}.zip");
                    string zipPath = $"{AttachmentsDirectory}/{zipName}";

                    ZipFile.CreateFromDirectory(ZipTempDirectory, zipPath);

                    new DirectoryInfo(ZipTempDirectory).Clear();
                    Attachment a = new Attachment
                    {
                        Id = $"{Guid.NewGuid()}",
                        FileName = zipName,
                        FilePath = zipPath
                    };
                    context.Attachments.Add(a);
                    context.SaveChanges();
                    return $"/File/DownloadByKey?key={a.Id}";
                }

                return "";
            }
        }
        #endregion

        
    }
}
