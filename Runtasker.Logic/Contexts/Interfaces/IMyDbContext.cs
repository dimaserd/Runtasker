﻿using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Interfaces
{
    public interface IMyDbContext
    {
        
        IDbSet<ApplicationUser> Users { get; }

        DbSet<OtherUserInfo> OtherUserInfos { get; set; }

        

        DbSet<Order> Orders { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<Attachment> Attachments { get; set; }

        DbSet<Payment> Payments { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<Invitation> Invitations { get; set; }

        DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        void SaveChanges();

        Task<int> SaveChangesAsync();
        
    }
}
