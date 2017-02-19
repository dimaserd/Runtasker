using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Runtasker.Logic
{
    public class IdentityClasses
    {

        private UserManager<ApplicationUser> usermanager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));

        private RoleManager<IdentityRole> RoleManager = 
            new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

        public UserManager<ApplicationUser> GetUserManager()
        {
            return usermanager;
        }
    }
}
