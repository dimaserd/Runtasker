using Extensions.String;
using Logic.Extensions.Models;
using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;
using Runtasker.Settings.Files;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Runtasker.Logic.Workers.Files
{
    
    public class CustomerFileMethods : FileWorkerBase
    {
        #region Конструкторы
        public CustomerFileMethods(string rootDirectory, MyDbContext context = null) : base(rootDirectory)
        {
            Construct(context);
        }

        public CustomerFileMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Attachmenter = new CustomerAttachmentWorker();
            Namer = new AttachmentNamer();
            _context = context;
        }
        #endregion

        #region Поля
        MyDbContext _context;
        #endregion

        #region Константы

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        void RewriteAttachmentsToOrder(int orderId)
        {
            using (MyDbContext context = new MyDbContext())
            {
                string zipPathWithFiles = WriteZipWithAllFilesInFolderToAttachmentsDirectory(GetOrderDirectoryPath(orderId));

                //a specail mark for getting an Attachment
                string mark = Namer.Mark.GetForOrder(orderId);

                //for getting name of previous zip archive
                Attachment orderA = context.Attachments.FirstOrDefault(a => a.OrderId == orderId);


                if (orderA == null)
                {
                    Attachment newOrderA = new Attachment()
                    {
                        FilePath = zipPathWithFiles,
                        FileName = zipPathWithFiles.leftJustFileName(),
                        OrderId = orderId
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

        #region Свойства

        CustomerAttachmentWorker Attachmenter { get; set; }

        AttachmentNamer Namer { get; set; }

        MyDbContext Context
        {
            get
            {
                if(_context == null)
                {
                    _context = new MyDbContext();
                }
                return _context;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Создает директорию для заказа и очищает и возвращает путь к данной директории
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string CreateOrderDirectory(int orderId)
        {
            //если директория не существует то все норм создаем новую
            //иначе вычистить всю старую
            string orderDirPath = $"{RootDirectory}/Orders/{orderId}";
            if (!Directory.Exists(orderDirPath))
            {
                Directory.CreateDirectory(orderDirPath);
            }
            else
            {
                new DirectoryInfo(orderDirPath).Clear();
            }
            return orderDirPath;
        }

        
        #endregion

        #region Методы по событиям

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WorkerResult OnCustomerAddedNewFilesToOrder(OrderAddFilesModel model)
        {
            string orderDirectoryPath = GetOrderDirectoryPath(model.OrderId);

            List<string> fileNamesInOrderDir = Directory.GetFiles(orderDirectoryPath).ToList().leftJustNames();

            int filesCount = 0;

            //из модели достаются только безопасные файлы
            List<HttpPostedFileBase> goodFiles = model.Files.Where(x => FilesSettings.IsThatGoodFile(x)).ToList();

            Attachment orderAttachment = Context.Attachments.FirstOrDefault(x => x.OrderId == model.OrderId);

            orderAttachment.AddFilesToAttachment(goodFiles);

            Context.Attachments.Attach(orderAttachment);
            Context.Entry(orderAttachment).State = EntityState.Modified;
            Context.SaveChanges();
            
            return new WorkerResult
            {
                Succeeded = true
            };
        }
        
        /// <summary>
        /// Записывает файлы заказа в директорию
        /// </summary>
        /// <param name="files"></param>
        /// <param name="order"></param>
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
                    if(FilesSettings.IsThatGoodFile(file))
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        file.SaveAs($"{OrderDirectory}/{fileName}");
                    } 
                }
            }

            string attachmentsZipPath = WriteZipWithAllFilesInFolderToAttachmentsDirectory(OrderDirectory);
            if(attachmentsZipPath != null)
            {
                Attachmenter.OnCustomerCreatedAnOrder(order, attachmentsZipPath);
            }
            
        }
        #endregion


        #region Вспомогательные методы
        
        #endregion
    }
}
