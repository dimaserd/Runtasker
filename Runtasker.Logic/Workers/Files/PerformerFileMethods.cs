
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;
using System.IO;
using System.Web;

namespace Runtasker.Logic.Workers.Files
{
    public class PerformerFileMethods : FileWorkerBase
    {
        #region Constructors
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
            Attachmenter = new PerformerAttachmentMethods();
        }

        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        PerformerAttachmentMethods Attachmenter { get; set; }
        #endregion

        #region Help Methods

        string GetOrderSolutionFolder(int orderId)
        {
            string result =  $"{GetOrderDirectoryPath(orderId)}/Solution";
            if(!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            return result;
        }

        #endregion

        #region Methods like Events
        //Saving files and writes attachments zip via attachmenter
        public void OnPerformerSolvedAnOrder(SolveOrderModel model)
        {
            string solutionDirectory = GetOrderSolutionFolder(model.OrderId);
            foreach (HttpPostedFileBase file in model.SolutionFiles)
            {
                if (file != null)
                {
                    file.SaveAs($"{solutionDirectory}/{Path.GetFileName(file.FileName)}");
                }
            }

            string zipPath = WriteZipWithAllFilesInFolderToAttachmentsDirectory(solutionDirectory);

            Attachmenter.OnPerformerSolvedAnOrder(zipPath, model.OrderId);

        }
        #endregion
    }
}
