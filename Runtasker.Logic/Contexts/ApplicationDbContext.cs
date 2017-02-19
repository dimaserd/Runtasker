using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Settings.Statics;

namespace Runtasker.Logic.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string connection)
            
            : base(connection, throwIfV1Schema: false)
        {
            if(connection != "LocalTestConnection")
            {
                throw new System.Exception("Нельзя сбрасывать не локальные базы данных!");
            }
        }

        /// <summary>
        /// строка подключения берется из проекта с настройками
        /// </summary>
        public ApplicationDbContext()
            : base(ConnectionStringStatic.ConnectionString, throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}
