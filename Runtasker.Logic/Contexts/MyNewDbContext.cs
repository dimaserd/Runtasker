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
        public new IDbSet<ApplicationUser> Users
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

        public new IDbSet<IdentityRole> Roles
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

        public new IDbSet<OtherUserInfo> OtherUserInfos
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

        public new IDbSet<Order> Orders
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

        public new IDbSet<Message> Messages
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

        public new IDbSet<Attachment> Attachments
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

        public new IDbSet<Payment> Payments
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

        public new IDbSet<Notification> Notifications
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

        public new IDbSet<Invitation> Invitations
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

        public new IDbSet<PaymentTransaction> PaymentTransactions
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
