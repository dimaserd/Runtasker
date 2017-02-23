using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts
{
    public class MyNewDbContext : MyDbContext, IMyDbContext
    {
        

        #region Properties
        public new DbSet<ApplicationUser> Users
        {
            get
            {
                return Users;
            }

            set
            {
                Users = value;
            }
        }

        public new DbSet<IdentityRole> Roles
        {
            get
            {
                return Roles;
            }

            set
            {
                Roles = value;
            }
        }

        public new DbSet<OtherUserInfo> OtherUserInfos
        {
            get
            {
                return OtherUserInfos;
            }
            set
            {
                OtherUserInfos = value;
            }
        }

        public new DbSet<Order> Orders
        {
            get
            {
                return Orders;
            }

            set
            {
                Orders = value;
            }
        }

        public new DbSet<Message> Messages
        {
            get
            {
                return Messages;
            }

            set
            {
                Messages = value;
            }
        }

        public new DbSet<Attachment> Attachments
        {
            get
            {
                return Attachments;
            }

            set
            {
                Attachments = value;
            }
        }

        public new DbSet<Payment> Payments
        {
            get
            {
                return Payments;
            }

            set
            {
                Payments = value;
            }
        }

        public new DbSet<Notification> Notifications
        {
            get
            {
                return Notifications;
            }

            set
            {
                Notifications = value;
            }
        }

        public new DbSet<Invitation> Invitations
        {
            get
            {
                return Invitations;
            }

            set
            {
                Invitations = value;
            }
        }

        public new DbSet<PaymentTransaction> PaymentTransactions
        {
            get
            {
                return PaymentTransactions;
            }

            set
            {
                PaymentTransactions = value;
            }
        }


        #endregion

        #region Methods
        public new void SaveChanges()
        {
            SaveChanges();
        }

        public new Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
        #endregion
    }
}
