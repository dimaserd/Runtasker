﻿using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Fake
{
    public class TestDbContext : IMyDbContext
    {
        #region Fields

        #region TestDbSets
        TestDbSet<ApplicationUser> _users;

        TestDbSet<IdentityRole> _roles;

        TestDbSet<OtherUserInfo> _otherUserInfos;

        TestDbSet<Order> _orders;

        TestDbSet<Message> _messages;

        TestDbSet<Attachment> _attachments;

        TestDbSet<Payment> _payments;

        TestDbSet<Notification> _notifications;

        TestDbSet<Invitation> _invitations;

        TestDbSet<PaymentTransaction> _paymentTransactions;
        #endregion

        #endregion

        #region Properties
        public int CountOfSaveChanges
        {
            get; private set;
            
        }

        #region DbSets
        public IDbSet<ApplicationUser> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new TestDbSet<ApplicationUser>();
                }
                return _users;
            }
        }

        public DbSet<IdentityRole> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new TestDbSet<IdentityRole>();
                }
                return _roles;
            }

            set
            {
                _roles = new TestDbSet<IdentityRole>();
            }
        }

        public DbSet<OtherUserInfo> OtherUserInfos
        {
            get
            {
                if (_otherUserInfos == null)
                {
                    _otherUserInfos = new TestDbSet<OtherUserInfo>();
                }
                return _otherUserInfos;
            }

            set
            {
                _otherUserInfos = null;
            }
        }

        public DbSet<Order> Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new TestDbSet<Order>();
                }
                return _orders;
            }

            set
            {
                _orders = null;
            }
        }

        public DbSet<Message> Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new TestDbSet<Message>();
                }
                return _messages;
            }

            set
            {
                _messages = null;
            }
        }

        public DbSet<Attachment> Attachments
        {
            get
            {
                if (_attachments == null)
                {
                    _attachments = new TestDbSet<Attachment>();
                }
                return _attachments;
            }

            set
            {
                _attachments = null;
            }
        }

        public DbSet<Payment> Payments
        {
            get
            {
                if (_payments == null)
                {
                    _payments = new TestDbSet<Payment>();
                }
                return _payments;
            }

            set
            {
                _payments = null;
            }
        }

        public DbSet<Notification> Notifications
        {
            get
            {
                if (_notifications == null)
                {
                    _notifications = new TestDbSet<Notification>();
                }
                return _notifications;
            }

            set
            {
                _notifications = null;
            }
        }

        public DbSet<Invitation> Invitations
        {
            get
            {
                if (_invitations == null)
                {
                    _invitations = new TestDbSet<Invitation>();
                }
                return _invitations;
            }

            set
            {
                _invitations = null;
            }
        }

        public DbSet<PaymentTransaction> PaymentTransactions
        {
            get
            {
                if (_paymentTransactions == null)
                {
                    _paymentTransactions = new TestDbSet<PaymentTransaction>();
                }
                return _paymentTransactions;
            }

            set
            {
                _paymentTransactions = null;
            }
        }

        
        public void SaveChanges()
        {
            CountOfSaveChanges++;
        }

        public async Task<int> SaveChangesAsync()
        {
            await Task.FromResult(0);
            CountOfSaveChanges++;

            return 0;
        }
        #endregion

        #endregion
    }
}
