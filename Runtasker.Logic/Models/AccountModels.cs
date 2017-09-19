using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Runtasker.Resources.Models.Account.LoginModel;
using Runtasker.Settings;
using System;
using System.Threading;
using Runtasker.Logic.Entities;

namespace Runtasker.Logic.Models
{
    public class AddEmailModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }
    }

    public class ExternalLoginConfirmationModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }

        //My addition for html emails
        public string ProviderName { get; set; }
    }

    public class ExternalLoginListModel
    {
        public string ReturnUrl { get; set; }

        public string VerbWith { get; set; }

        /// <summary>
        /// По нему определяется куда нужно вставлять имя социальной сети
        /// например $"{0}{socialName}{1}"
        /// </summary>
        public string Pattern { get; set; }
    }

    public class SendCodeModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code", ResourceType = typeof(AccountRes))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "RememberBrowser", ResourceType = typeof(AccountRes))]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(AccountRes))]
        public bool RememberMe { get; set; }
    }

    #region Register Models

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PassRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [StringLength(100, ErrorMessage = "Password {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "Name", ResourceType = typeof(AccountRes))]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPass", ResourceType = typeof(AccountRes))]
        [Compare("Password", ErrorMessageResourceName = "PassMatchError", ErrorMessageResourceType = typeof(AccountRes))]
        public string ConfirmPassword { get; set; }
    }

    
    public class RegisterByInvitationModel : RegisterModel
    {
        public string InvitationId { get; set; }
    }

    public static class RegisterModelExtensionMethods
    {
        public static ApplicationUser ToCustomer(this RegisterModel model)
        {
            return new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = false,
                Balance = UISettings.RegistrationBonus,
                RegistrationDate = DateTime.Now,
                Language = Thread.CurrentThread.CurrentCulture.Name
            };
        }
        

    }
    #endregion

    public class ResetPasswordModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PassRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [StringLength(100, ErrorMessage = "Password {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPass", ResourceType = typeof(AccountRes))]
        [Compare("Password", ErrorMessageResourceName = "PassMatchError", ErrorMessageResourceType = typeof(AccountRes))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(AccountRes))]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }
    }
}
