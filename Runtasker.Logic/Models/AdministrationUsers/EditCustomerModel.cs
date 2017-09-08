using Runtasker.Logic.Entities;
using Runtasker.Logic.Interfaces.Identity;
using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Models.AdministrationUsers
{
    public class EditCustomerModel : IVkUser
    {
        public string UserId { get; set; }

        
        public string Email { get; set; }

        [Display(Name = "Почта подтверждена")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }

        [Display(Name = "Уведомлять в вк")]
        public bool ShouldBeNotifictedInVk { get; set; }

        
    }

    public static class EditCustomerModelExtensions
    {
        public static EditCustomerModel ToEditCustomerModel(this ApplicationUser user)
        {
            return new EditCustomerModel
            {
                UserId = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                VkDomain = user.VkDomain,
                VkId = user.VkId,
                ShouldBeNotifictedInVk = user.ShouldBeNotifictedInVk,
                Name = user.Name
            };
        }

        public static ApplicationUser RenderApplicationUser(this EditCustomerModel model, ApplicationUser user)
        {
            user.Email = model.Email;
            user.EmailConfirmed = model.EmailConfirmed;
            user.Name = model.Name;
            user.VkDomain = model.VkDomain;
            user.VkId = model.VkId;
            user.ShouldBeNotifictedInVk = model.ShouldBeNotifictedInVk;

            return user;
        }
    }
}
