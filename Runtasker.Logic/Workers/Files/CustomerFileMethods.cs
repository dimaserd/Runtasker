using Extensions.String;
using Logic.Extensions.Models;
using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Runtasker.Logic.Workers.Files
{
    //TODO delete Context from here it must be in attachments worker
    public class CustomerFileMethods : FileWorkerBase
    {
        #region Constructors
        public CustomerFileMethods(string rootDirectory) : base(rootDirectory)
        {
            Construct();
        }

        public CustomerFileMethods()
        {
            Construct();
        }

        void Construct()
        {
            Attachmenter = new CustomerAttachmentWorker();
            Namer = new AttachmentNamer();
        }
        #endregion

        #region Help Methods

        //This must be in customer attachment worker
        //Rewrites zip in attachments folder with the same name
        //we could left the previous name

        void RewriteAttachmentsToOrder(int orderId)
        {
            using (MyDbContext context = new MyDbContext())
            {
                string zipPathWithFiles = WriteZipWithAllFilesInFolderToAttachmentsDirectory(GetOrderDirectoryPath(orderId));

                //a specail mark for getting an Attachment
                string mark = Namer.Mark.GetForOrder(orderId);

                //for getting name of previous zip archive
                Attachment orderA = context.Attachments.FirstOrDefault(a => a.Mark == mark);


                if (orderA == null)
                {
                    Attachment newOrderA = new Attachment()
                    {
                        FilePath = zipPathWithFiles,
                        FileName = zipPathWithFiles.leftJustFileName(),
                        Mark = mark
                    };
                    Order order = context.Orders.FirstOrDefault(o => o.Id == orderId);
                    order.Attachments = $"/File/DownloadByKey?key={newOrderA.Id}";

                    context.Attachments.Add(newOrderA);

                    context.SaveChanges();
                    return;
                }
                string previousZipPath = orderA.FilePath;
                //deleting previous zip archive
                File.Delete(previousZipPath);
                File.Move(zipPathWithFiles, previousZipPath);
            }
        }

        #endregion

        #region Private Properties

        CustomerAttachmentWorker Attachmenter { get; set; }

        AttachmentNamer Namer { get; set; }
        #endregion

        #region Public Methods
        public string CreateOrderDirectory(int orderId)
        {
            if (!Directory.Exists($"{RootDirectory}/Orders/{orderId}"))
            {
                Directory.CreateDirectory($"{RootDirectory}/Orders/{orderId}");
            }
            return $"{RootDirectory}/Orders/{orderId}";
        }

        
        #endregion

        #region Methods Like Events

        //adding new files to existing directory
        //rewriting zip with all customer Files
        public WorkerResult OnCustomerAddedNewFilesToOrder(OrderAddFilesModel model)
        {
            string orderDirectoryPath = GetOrderDirectoryPath(model.OrderId);

            List<string> fileNamesInOrderDir = Directory.GetFiles(orderDirectoryPath).ToList().leftJustNames();

            int filesCount = 0;
            foreach(HttpPostedFileBase file in model.Files)
            {
                if (file != null)
                {
                    filesCount++;
                    string uniqueFileName = file.FileName.makeFileNameUniqueAtList(fileNamesInOrderDir);
                    string filePath = $"{orderDirectoryPath}/{uniqueFileName}";
                    file.SaveAs(filePath);
                }
            }
            if(filesCount == 0)
            {
                return new WorkerResult("No files were attached!");
            }

            RewriteAttachmentsToOrder(model.OrderId);
            return new WorkerResult
            {
                Succeeded = true
            };
        }
        
        public void OnCustomerCreatedAnOrder(IEnumerable<HttpPostedFileBase> files, Order order)
        {
            //Saving all files to directory
            string OrderDirectory = CreateOrderDirectory(order.Id);

            if(files == null)
            {
                return;
            }

            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs($"{OrderDirectory}/{fileName}");
                }
            }

            string attachmentsZipPath = WriteZipWithAllFilesInFolderToAttachmentsDirectory(OrderDirectory);
            if(attachmentsZipPath != null)
            {
                Attachmenter.OnCustomerCreatedAnOrder(order, attachmentsZipPath);
            }
            
        }
        #endregion
    }
}
