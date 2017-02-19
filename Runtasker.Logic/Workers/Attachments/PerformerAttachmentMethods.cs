
using Runtasker.Logic.Entities;
using System;

namespace Runtasker.Logic.Workers.Attachments
{
    public class PerformerAttachmentMethods : AttachmentWorkerBase
    {
        #region Constructors
        public PerformerAttachmentMethods() : base()
        {

        }
        #endregion

        #region Methods like Events
        public void OnPerformerSolvedAnOrder(string zipPath, int orderId)
        {
            using (MyDbContext context = new MyDbContext())
            {
                string key = Guid.NewGuid().ToString();
                Attachment a = new Attachment
                {
                    Id = key,
                    FilePath = zipPath,
                    FileName = $"OrderSolution№{orderId}.zip",
                    Mark = Namer.Mark.GetForOrderSolution(orderId)
                };
                context.Attachments.Add(a);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
