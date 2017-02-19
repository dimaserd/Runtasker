using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Runtasker.Logic.Workers.Developer
{
    //This class can create users and mao some operations 
    //that users can do in real application
    public class DeveloperMoqMethods
    {
        #region Constructors
        public DeveloperMoqMethods(MyDbContext context)
        {
            Context = context;
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
        }
        #endregion

        #region Private Properties

        static MyDbContext Context { get; set; }

        UserManager<ApplicationUser> userManager { get; set; }

        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

        #endregion

        #region Public Methods
        //Creates User or returns information that it already exists
        public string CreateUser()
        {
            string email = "test@mail.com";
            string password = "testpassword";


            ApplicationUser createdUser = userManager.FindByEmail(email);

            if(createdUser != null)
            {
                return $"Пользователь уже существует";
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                Balance = 300.00m,
                Name = "TestName",
                UserName = "TestUserName",
                EmailConfirmed = true
            };


            IdentityResult result = userManager.Create(user, password);
            if(result.Succeeded)
            {
                return $"Создан пользователь, email: {user.Email} pass: {password}";
            }
            else
            {
                string result_string = "";
                foreach(string s in result.Errors)
                {
                    result_string += (s + "\n");
                }
                return result_string;
            }

            
        }

        public string AddUserToRole(string email, string role)
        {
            ApplicationUser user = userManager.FindByEmail(email);
            if(user == null)
            {
                return $"пользователя с email: {email} не существует";
            }
            bool roleExists = roleManager.FindByName(role) != null;
            if(!roleExists)
            {
                string result = "выбранной вами роли не существует\n посмотрите существующие роли ниже\n";
                foreach(var _role in roleManager.Roles)
                {
                    result += (_role.Name.ToString() + "\n");
                }
                return result;
            }
            else
            {
                userManager.AddToRole(user.Id, role);
                return $"пользователю email:{user.Email} присвоена роль {role}";
            }
            
        }
        #endregion
    }
}
