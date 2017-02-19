using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace Runtasker.Logic.Workers.Attachments
{
    public class ContactAttachmentMethods : AttachmentWorkerBase
    {
        #region Constructors
        public ContactAttachmentMethods() : base()
        {

        }
        #endregion

        #region Public Methods
        //Maybe we should generate domains 
        public string GetAttachmentsLink(IEnumerable<HttpPostedFileBase> files)
        {
            using (MyDbContext context = new MyDbContext())
            {
                new DirectoryInfo(ZipTempDirectory).Clear();
                if (files == null)
                {
                    return "";
                }
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        file.SaveAs($"{ZipTempDirectory}/{file.FileName}");
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
                return $"runtasker.ru/File/DownloadByKey?key={a.Id}";
            }
        }
        #endregion

        #region Private Methods
        //TO DO separate methods
        //This not working
        private string CreateUniqueZip()
        {
            using (MyDbContext context = new MyDbContext())
            {
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
        }
        #endregion
    }
}
