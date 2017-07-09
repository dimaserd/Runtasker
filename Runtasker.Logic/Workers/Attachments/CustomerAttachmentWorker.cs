using Runtasker.Logic.Entities;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Attachments
{
    public class CustomerAttachmentWorker : AttachmentWorkerBase
    {
        #region Constructors
        public CustomerAttachmentWorker() : base()
        {
        }
        
        #endregion

        #region Private Help Methods
        private string GetOrderDirectory(Order order)
        {
            return $"{OrdersDirectory}/{order.Id}";
        }
        #endregion

        #region Methods like Events

        public void OnCustomerCreatedAnOrder(Order order, string attachmentsZipPath)
        {
            using (MyDbContext context = new MyDbContext())
            {
                string key = Guid.NewGuid().ToString();
                Attachment a = new Attachment
                {
                    Id = key,
                    FileName = $"Order№{order.Id}Files.zip",
                    FilePath = attachmentsZipPath,
                    
                };
                context.Attachments.Add(a);

                Order newOrder = context.Orders.FirstOrDefault(o => o.Id == order.Id);
                
                context.SaveChanges();
            }
        }

        #endregion
    }
}
