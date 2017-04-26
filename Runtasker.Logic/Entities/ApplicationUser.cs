using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Interfaces.Identity;
using Runtasker.Logic.Models.ManageModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Runtasker.Logic.Entities
{

    public class ApplicationUser : IdentityUser, IVkUser
    {
        #region Конструкторы
        public ApplicationUser()
        {
            Payments = new List<Payment>();
            PaymentTransactions = new List<PaymentTransaction>();
        }
        #endregion

        #region Свойства

        #region IVkUser свойства
        public string VkDomain { get; set; }

        public string VkId { get; set; }
        #endregion

        public string Specialization { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public DateTime? RegistrationDate { get; set; }

        #endregion


        /// <summary>
        /// Добавление клеймов по свойствам
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
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
            userIdentity.AddClaim(new Claim("PhoneNumber", PhoneNumber == null ? string.Empty : PhoneNumber.ToString()));

            userIdentity.AddClaim(new Claim("Specialization", Specialization == null? string.Empty : Specialization.ToString()));
            userIdentity.AddClaim(new Claim("VkDomain", VkDomain == null ? string.Empty : VkDomain.ToString()));
            userIdentity.AddClaim(new Claim("VkId", VkId == null ? string.Empty : VkId.ToString()));

            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        #region Отношения к коллекциям

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        
        public virtual ICollection<Coupon> Coupons { get; set; }
        #endregion
    }

    #region Расширения

    public static class ApplicationUserExtensions
    {
        
    }

    /// <summary>
    /// Идентфикационные расширения
    /// </summary>
    public static class MyIdentityExtensions
    {
        #region Список клеймов

        /// <summary>
        /// Получает предметы, которые может выполнять исполнитель из свойства Specialization
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static IEnumerable<Subject> GetPerformerSubjects(this IIdentity identity)
        {
            OtherUserInfo info = GetOtherInfo(identity);

            return info.GetSubjects();
        }

        public static OtherUserInfo GetOtherInfo(this IIdentity identity)
        {
            return new OtherUserInfo
            {
                UserId = identity.GetUserId(),
                Specialization = GetClaimValueByName(identity, "Specialization"),
                VkId = GetClaimValueByName(identity, "VkId"),
                VkDomain = GetClaimValueByName(identity, "VkDomain")
            };
        }

        public static string GetPhoneNumber(this IIdentity identity)
        {
            return GetClaimValueByName(identity, "PhoneNumber");
        }

        public static string GetName(this IIdentity identity)
        {
            return GetClaimValueByName(identity, "Name");

        }

        public static string GetLanguage(this IIdentity identity)
        {
            return GetClaimValueByName(identity, "Language");
        }

        public static bool IsEmailConfirmed(this IIdentity identity)
        {
            string claimValue = GetClaimValueByName(identity, "EmailConfirmed");
            if (claimValue != null && claimValue == "true")
            {
                return true;
            }

            return false;
        }

        public static bool HasPassword(this IIdentity identity)
        {
            string pass = GetClaimValueByName(identity, "Password");
            return (pass != null) ? (!string.IsNullOrEmpty(pass)) : false;
        }

        public static string GetBalance(this IIdentity identity)
        {
            return GetClaimValueByName(identity, "Balance");
        }

        public static string GetEmail(this IIdentity identity)
        {
            return GetClaimValueByName(identity, "Email");
        }
        #endregion

        #region Вспомогательные методы
        private static string GetClaimValueByName(this IIdentity identity, string claimName)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(claimName);
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        #endregion
    }
    #endregion
}
