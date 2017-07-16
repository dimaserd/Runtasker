
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;

namespace Runtasker.Logic.Workers.Files
{
    public class PerformerFileMethods : FileWorkerBase
    {
        #region Конструкторы
        public PerformerFileMethods(MyDbContext context) : base()
        {
            Construct(context);
        }

        public PerformerFileMethods(MyDbContext context, string rootDirectory) : base(rootDirectory)
        {
            Construct(context);
        }

        //Construct methods pass MyDbContext to attachmenter
        void Construct(MyDbContext context)
        {
            Context = context;
        }

        #endregion

        #region Свойства
        MyDbContext Context { get; set; }
        #endregion

        

        #region Публичные методы
        //Saving files and writes attachments zip via attachmenter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void OnPerformerSolvedAnOrder(SolveOrderModel model, SaveChangesType saveType = SaveChangesType.Now)
        {
            Attachment solution = AttachmentExtensions.GetAttachmentFromFiles(model.SolutionFiles);

            solution.Type = AttachmentType.OrderSolution;
            solution.OrderId = model.OrderId;

            Context.Attachments.Add(solution);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }
        }
        #endregion
    }
}
