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
           
            _context = context;
        }
        #endregion

        #region Поля
        MyDbContext _context;
        #endregion

        #region Константы

        #endregion

       

        #region Свойства

        CustomerAttachmentWorker Attachmenter { get; set; }

        
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
