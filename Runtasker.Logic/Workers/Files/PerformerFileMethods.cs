
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;

namespace Runtasker.Logic.Workers.Files
{
    public class PerformerFileMethods : FileWorkerBase
    {
        #region Конструкторы
        public PerformerFileMethods(IMyDbContext context) : base()
        {
            Construct(context);
        }

        public PerformerFileMethods(IMyDbContext context, string rootDirectory) : base(rootDirectory)
        {
            Construct(context);
        }

        //Construct methods pass MyDbContext to attachmenter
        void Construct(IMyDbContext context)
        {
            Context = context;
        }

        #endregion

        #region Свойства
        IMyDbContext Context { get; set; }
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
