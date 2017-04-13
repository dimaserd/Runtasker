using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using Runtasker.Logic.Models.ManageModels;

namespace Runtasker.Logic.Contexts.Fake
{
    public class TestDbContext : IMyDbContext
    {
        #region Fields

        #region TestDbSets
        TestDbSet<ApplicationUser> _users;

        TestDbSet<IdentityRole> _roles;

        

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

        public IDbSet<IdentityRole> Roles
        {
            get
            {
                if(_roles == null)
                {
                    _roles = new TestDbSet<IdentityRole>();
                }
                return _roles;
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

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~TestDbContext() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
             GC.SuppressFinalize(this);
        }
        #endregion
    }
}
