using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Interfaces
{
    public interface IMyDbContext: IDisposable
    {
        
        IDbSet<ApplicationUser> Users { get; }

        IDbSet<IdentityRole> Roles { get; }

        DbSet<OtherUserInfo> OtherUserInfos { get; }

        

        DbSet<Order> Orders { get;  }

        DbSet<Message> Messages { get;  }

        DbSet<Attachment> Attachments { get; }

        DbSet<Payment> Payments { get; }

        DbSet<Notification> Notifications { get; }

        DbSet<Invitation> Invitations { get; }

        DbSet<PaymentTransaction> PaymentTransactions { get; }

        void SaveChanges();

        Task<int> SaveChangesAsync();
        
    }
}
