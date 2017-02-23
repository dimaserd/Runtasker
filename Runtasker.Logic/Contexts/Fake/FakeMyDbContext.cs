using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Runtasker.Logic.Contexts.Fake
{
    public class FakeMyDbContext : MyDbContext
    {
        #region Methods
        public static FakeMyDbContext CreateDatabase()
        {
            FakeMyDbContext db = new FakeMyDbContext();

            ApplicationUser performer = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "dimaserd84@gmail.com",
                EmailConfirmed = true,
                UserName = "dimaserd84@gmail.com",
                Name = "Dmitry"

            };

            ApplicationUser customer = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "dimaserd96@yandex.ru",
                EmailConfirmed = true,
                UserName = "dimaserd96@yandex.ru",
                Name = "Dmitry",
                Balance = 300
                
            };

            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(performer);
            users.Add(customer);

            db.Users.AddRange(users);

            return db;
        }
        #endregion

        #region Fields
        DbSet<ApplicationUser> _users;

        DbSet<IdentityRole> _roles;

        DbSet<OtherUserInfo> _otherUserInfos;

        DbSet<Order> _orders;

        DbSet<Message> _messages;

        DbSet<Attachment> _attachments;

        DbSet<Payment> _payments;

        DbSet<Notification> _notifications;

        DbSet<Invitation> _invitations;

        DbSet<PaymentTransaction> _paymentTransactions;

        #endregion

        #region Properties
        public new DbSet<ApplicationUser> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new FakeDbSet<ApplicationUser>();
                }
                return _users;
            }

            set
            {
                _users = null;
            }
        }

        public new DbSet<IdentityRole> Roles
        {
            get
            {
                if(_roles == null)
                {
                    _roles = new FakeDbSet<IdentityRole>();
                }
                return _roles;
            }

            set
            {
                _roles = new FakeDbSet<IdentityRole>();
            }
        }

        public new DbSet<OtherUserInfo> OtherUserInfos
        {
            get
            {
                if(_otherUserInfos == null)
                {
                    _otherUserInfos = new FakeDbSet<OtherUserInfo>();
                }
                return _otherUserInfos;
            }

            set
            {
                _otherUserInfos = null;
            }
        }

        public new DbSet<Order> Orders
        {
            get
            {
                if(_orders == null)
                {
                    _orders = new FakeDbSet<Order>();
                }
                return _orders;
            }

            set
            {
                _orders = null;
            }
        }

        public new DbSet<Message> Messages
        {
            get
            {
                if(_messages == null)
                {
                    _messages = new FakeDbSet<Message>();
                }
                return _messages;
            }

            set
            {
                _messages = null;
            }
        }

        public new DbSet<Attachment> Attachments
        {
            get
            {
                if(_attachments == null)
                {
                    _attachments = new FakeDbSet<Attachment>();
                }
                return _attachments;
            }

            set
            {
                _attachments = null;
            }
        }

        public new DbSet<Payment> Payments
        {
            get
            {
                if(_payments == null)
                {
                    _payments = new FakeDbSet<Payment>();
                }
                return _payments;
            }

            set
            {
                _payments = null;
            }
        }

        public new DbSet<Notification> Notifications
        {
            get
            {
                if(_notifications == null)
                {
                    _notifications = new FakeDbSet<Notification>();
                }
                return _notifications;
            }

            set
            {
                _notifications = null;
            }
        }

        public new DbSet<Invitation> Invitations
        {
            get
            {
                if(_invitations == null)
                {
                    _invitations = new FakeDbSet<Invitation>();
                }
                return _invitations;
            }

            set
            {
                _invitations = null;
            }
        }

        public new DbSet<PaymentTransaction> PaymentTransactions
        {
            get
            {
                if(_paymentTransactions == null)
                {
                    _paymentTransactions = new FakeDbSet<PaymentTransaction>();
                }
                return _paymentTransactions;
            }

            set
            {
                _paymentTransactions = null;
            }
        }
        #endregion

        
    }
}
