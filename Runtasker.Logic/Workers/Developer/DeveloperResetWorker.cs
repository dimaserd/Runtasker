using Runtasker.Logic.Workers.Files;
using System.IO;

namespace Runtasker.Logic.Workers.Developer
{
    public class DeveloperResetWorker
    {
        #region Constructors
        public DeveloperResetWorker(MyDbContext context)
        {
            Context = context;
        }
        #endregion

        #region Private Fields
        private MyDbContext Context { get; set; }
        #endregion

        #region Public Methods
        public string Orders()
        {
            Context.Orders.RemoveRange(Context.Orders);
            Context.SaveChanges();

            //Context.Orders.SqlQuery("ALTER TABLE dbo.Orders ALTER COLUMN Id COUNTER(1,1)");
            Context.SaveChanges();
            RemoveDirectories();
            return "Orders";
        }

        public string Users()
        {
            foreach (ApplicationUser user in Context.Users)
            {
                Context.Users.Remove(user);
            }
            Context.SaveChanges();
            return "Users";
        }

        public string Messages()
        {
            Context.Messages.RemoveRange(Context.Messages);
            Context.Attachments.RemoveRange(Context.Attachments);
            Context.SaveChanges();
            return "Messages";
        }
        #endregion

        #region Private Methods
        private void RemoveDirectories()
        {
            DirectoryInfo di = new DirectoryInfo($"{System.Web.Hosting.HostingEnvironment.MapPath("~/Files")}/Orders");
            di.Clear();
        }
        #endregion
    }
}
