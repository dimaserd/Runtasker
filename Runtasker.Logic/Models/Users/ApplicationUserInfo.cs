using Runtasker.Logic.Entities;

namespace Runtasker.Logic.Models.Users
{

    /// <summary>
    /// Класс описывающий некоторые свойства пользователя системы
    /// </summary>
    public class ApplicationUserInfo
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    public static class ApplicationUserInfoExtensions
    {
        public static ApplicationUserInfo ToApplicationUserInfo(this ApplicationUser user)
        {
            return new ApplicationUserInfo
            {
                Name = user.Name,
                Email = user.Email,
                UserId = user.Id
            };
        }
    }
}
