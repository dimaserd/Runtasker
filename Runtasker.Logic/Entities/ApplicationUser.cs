using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Entities
{

    public static class OtherUserInfoExtensionMethods
    {
        public static IEnumerable<Subject> GetPerformerSubjects(this  info)
        {
            List<Subject> allSubjects = Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();

            List<int> subjectInts = GetSubjectIntsFromSpecializationString(info.Specialization);

            List<Subject> result = new List<Subject>();

            foreach (Subject subject in allSubjects)
            {
                if (subjectInts.Contains((int)subject))
                {
                    result.Add(subject);
                }
            }

            return result;
        }


        public static IEnumerable<Order> GetOrdersForPerformerByInfo(this OtherUserInfo info, IEnumerable<Order> orders)
        {
            List<Subject> performerSubjects = GetPerformerSubjects(info).ToList();

            List<Order> result = new List<Order>();
            foreach (Order order in orders)
            {
                //если в списке предметов исполнителя содержиться
                //предмет заказа или предмет точно не указан
                //то добавляем предмет в результат
                if (performerSubjects.Any(x => x == order.Subject) || (order.Subject == Subject.Other))
                {
                    result.Add(order);
                }
            }

            return result;
        }

        
    }
    public static class MyIdentityExtensions
    {
        #region Claims list
        public static IEnumerable<Subject> GetPerformerSubjects(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Specialization");

            string specString = (claim != null) ? claim.Value : string.Empty;

            List<Subject> allSubjects = Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();

            List<int> subjectInts = GetSubjectIntsFromSpecializationString(specString);

            List<Subject> result = new List<Subject>();

            foreach (Subject subject in allSubjects)
            {
                if (subjectInts.Contains((int)subject))
                {
                    result.Add(subject);
                }
            }

            return result;
        }



        public static string GetPhoneNumber(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("PhoneNumber");
            return (claim != null) ? claim.Value : string.Empty;
        }

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
            if (claim != null && claim.Value == "true")
            {
                return true;
            }

            return false;
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

        #region Help Methods
        static List<int> GetSubjectIntsFromSpecializationString(string specString)
        {
            return specString
                .Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    int parseResult;
                    if (int.TryParse(x, out parseResult))
                    {
                        return parseResult;
                    }
                    else
                    {
                        return 0;
                    }
                }).Where(x => x > 0).ToList();
        }
        #endregion
    }

    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser, IVkUser
    {
        #region Constructors
        public ApplicationUser()
        {
            Payments = new List<Payment>();
            PaymentTransactions = new List<PaymentTransaction>();
            //VkPostLookUps = new List<VkPostLookUp>();
        }
        #endregion

        #region IVkUser свойства
        public string VkDomain { get; set; }

        public string VkId { get; set; }
        #endregion

        public string Specialization { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public OtherUserInfo OtherInfo { get; set; }


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

            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        #region Virtual collections

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        //public virtual ICollection<VkPostLookUp> VkPostLookUps { get; set; }

        //сообщения и заказы должны отсутствовать так как
        //это приведет к неведомой хуйне, а именно к созданию
        //колонки ApplicationUser_Id в подключенной через коллекцию
        //таблицу
        #endregion
    }
}
