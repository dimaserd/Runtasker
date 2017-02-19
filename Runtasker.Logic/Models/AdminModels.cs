using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Runtasker.Logic.Models
{
    public class CreateUserModel
    {
        #region Constructor
        public CreateUserModel()
        {
            SubjectsList = new MultiSelectList(Enum.GetValues(typeof(Subject)).Cast<Subject>());
        }
        #endregion
        [Display(Name = "Роли/Права")]
        public IEnumerable<string> UserRoles { get; set; }


        public MultiSelectList UserRolesList { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Исполняемые Предметы")]
        public IEnumerable<Subject> Subjects { get; set; }

        public MultiSelectList SubjectsList { get; set; }
    }

    public class AddUserToRoleModel
    {
        public ApplicationUser User { get; set; }

        public string RoleName { get; set; }
    }

    #region Info Models
    public class CustomerInfo
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        public IEnumerable<PaymentTransaction> PaymentTransactions { get; set; }
    }

    public class PerformerInfo
    {
        #region Constructor
        public PerformerInfo(ApplicationUser user, IEnumerable<Message> messages,
            IEnumerable<Order> orders, IEnumerable<PaymentTransaction> pts, OtherUserInfo info)
        {
            MainInfo = new UserInfo(user, messages, orders, pts);

            Construct(info);
        }

        void Construct(OtherUserInfo info)
        {
            Subjects = info.GetPerformerSubjects();
        }
        #endregion

        #region Properties
        public UserInfo MainInfo { get; set; }

        [Display(Name = "Выполняемые предметы")]
        public IEnumerable<Subject> Subjects { get; set; }
        #endregion

    }

    public class UserInfo
    {
        #region Constructor
        public UserInfo(ApplicationUser user, IEnumerable<Message> messages,
            IEnumerable<Order> orders, IEnumerable<PaymentTransaction> pts)
        {

            Balance = user.Balance;
            RegistrationDate = user.RegistrationDate;
            Email = user.Email;
            Id = user.Id;
            Name = user.Name;

            Messages = messages;
            Orders = orders;
            PaymentTransactions = pts;
            
        }
        #endregion

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        public IEnumerable<PaymentTransaction> PaymentTransactions { get; set; }
    }
    #endregion

    #region Email Models
    public class ActionEmailToCustomer
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public string Subject { get; set; }

        public string ActionButtonText { get; set; }

        public string ActionButtonLink { get; set; }
    }
    #endregion


}
