using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Collections.Generic;
using Runtasker.Logic.Entities;
using System;
using VkParser.Entities;

namespace Runtasker.Logic
{
    public static class MyIdentityExtensions
    {
        #region Claims list
        public static string GetName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Name");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetLanguage(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Language");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static bool IsEmailConfirmed(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("EmailConfirmed");
            if(claim != null && claim.Value == "true")
            {
                return true;
            }
           
            return  false;
        }

        public static bool HasPassword(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Password");
            return (claim != null) ? (!string.IsNullOrEmpty(claim.Value)) : false;
        }

        public static string GetBalance(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Balance");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Email");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        #endregion
    }

    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        #region Constructors
        public ApplicationUser()
        {
            
            Payments = new List<Payment>();
            PaymentTransactions = new List<PaymentTransaction>();
            VkPostLookUps = new List<VkPostLookUp>();
        }
        #endregion

        public string Language { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here => this.OrganizationId is a value stored in database against the user
            userIdentity.AddClaim(new Claim("Name", this.Name.ToString()));
            userIdentity.AddClaim(new Claim("Balance", Balance.ToString()));
            userIdentity.AddClaim(new Claim("Email", Email.ToString()));
            userIdentity.AddClaim(new Claim("Password", PasswordHash == null ? "" : PasswordHash.ToString()));
            userIdentity.AddClaim(new Claim("EmailConfirmed", EmailConfirmed == true ? "true" : "false"));
            userIdentity.AddClaim(new Claim("Language", Language));
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
        #region Virtual collections
        
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual ICollection<VkPostLookUp> VkPostLookUps { get; set; }
        
        //сообщения и заказы должны отсутствовать так как
        //это приведет к неведомой хуйне, а именно к созданию
        //колонки ApplicationUser_Id в подключенной через коллекцию
        //таблицу
        #endregion
    }

}