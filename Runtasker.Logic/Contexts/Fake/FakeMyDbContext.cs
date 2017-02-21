﻿using FakeDbSet;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Fake
{
    public class FakeMyDbContext : IMyDbContext
    {
        #region Fields
        InMemoryDbSet<ApplicationUser> _users;

        InMemoryDbSet<IdentityRole> _roles;

        InMemoryDbSet<OtherUserInfo> _otherUserInfos;

        InMemoryDbSet<Order> _orders;

        InMemoryDbSet<Message> _messages;

        InMemoryDbSet<Attachment> _attachments;

        InMemoryDbSet<Payment> _payments;

        InMemoryDbSet<Notification> _notifications;

        InMemoryDbSet<Invitation> _invitations;

        InMemoryDbSet<PaymentTransaction> _paymentTransactions;

        #endregion

        #region Properties
        public IDbSet<ApplicationUser> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new InMemoryDbSet<ApplicationUser>();
                }
                return _users;
            }

            set
            {
                _users = null;
            }
        }

        public IDbSet<IdentityRole> Roles
        {
            get
            {
                if(_roles == null)
                {
                    _roles = new InMemoryDbSet<IdentityRole>();
                }
                return _roles;
            }

            set
            {
                _roles = null;
            }
        }

        public IDbSet<OtherUserInfo> OtherUserInfos
        {
            get
            {
                if(_otherUserInfos == null)
                {
                    _otherUserInfos = new InMemoryDbSet<OtherUserInfo>();
                }
                return _otherUserInfos;
            }

            set
            {
                _otherUserInfos = null;
            }
        }

        public IDbSet<Order> Orders
        {
            get
            {
                if(_orders == null)
                {
                    _orders = new InMemoryDbSet<Order>();
                }
                return _orders;
            }

            set
            {
                _orders = null;
            }
        }

        public IDbSet<Message> Messages
        {
            get
            {
                if(_messages == null)
                {
                    _messages = new InMemoryDbSet<Message>();
                }
                return _messages;
            }

            set
            {
                _messages = null;
            }
        }

        public IDbSet<Attachment> Attachments
        {
            get
            {
                if(_attachments == null)
                {
                    _attachments = new InMemoryDbSet<Attachment>();
                }
                return _attachments;
            }

            set
            {
                _attachments = null;
            }
        }

        public IDbSet<Payment> Payments
        {
            get
            {
                if(_payments == null)
                {
                    _payments = new InMemoryDbSet<Payment>();
                }
                return _payments;
            }

            set
            {
                _payments = null;
            }
        }

        public IDbSet<Notification> Notifications
        {
            get
            {
                if(_notifications == null)
                {
                    _notifications = new InMemoryDbSet<Notification>();
                }
                return _notifications;
            }

            set
            {
                _notifications = null;
            }
        }

        public IDbSet<Invitation> Invitations
        {
            get
            {
                if(_invitations == null)
                {
                    _invitations = new InMemoryDbSet<Invitation>();
                }
                return _invitations;
            }

            set
            {
                _invitations = null;
            }
        }

        public IDbSet<PaymentTransaction> PaymentTransactions
        {
            get
            {
                if(_paymentTransactions == null)
                {
                    _paymentTransactions = new InMemoryDbSet<PaymentTransaction>();
                }
                return _paymentTransactions;
            }

            set
            {
                _paymentTransactions = null;
            }
        }
        #endregion

        public void SaveChanges()
        {

        }

        public async Task<int> SaveChangesAsync()
        {
            await Task.FromResult(0);
            return 0;
        }
    }
}
