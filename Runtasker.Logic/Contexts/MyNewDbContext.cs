using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts
{
    //public class MyNewDbContext : MyDbContext, IMyDbContext
    //{
    //    #region Fiels
    //    DbSet<Notification> _notifications;
    //    #endregion

    //    #region Properties
    //    public override IDbSet<ApplicationUser> Users
    //    {
    //        get
    //        {
    //            return base.Users;
    //        }

            
    //    }

        

    //    public override DbSet<OtherUserInfo> OtherUserInfos
    //    {
    //        get
    //        {
    //            return base.OtherUserInfos;
    //        }
            
    //    }

    //    public override DbSet<Order> Orders
    //    {
    //        get
    //        {
    //            return base.Orders;
    //        }

            
    //    }

    //    public override DbSet<Message> Messages
    //    {
    //        get
    //        {
    //            return base.Messages;
    //        }

            
    //    }

    //    public override DbSet<Attachment> Attachments
    //    {
    //        get
    //        {
    //            return base.Attachments;
    //        }

            
    //    }

    //    public override DbSet<Payment> Payments
    //    {
    //        get
    //        {
    //            return base.Payments;
    //        }

            
    //    }

    //    public new DbSet<Notification> Notifications
    //    {
    //        get
    //        {
    //            if(_notifications == null)
    //            {
                    
    //            }
    //            return base.Notifications;
    //        }

            
    //    }

    //    public override DbSet<Invitation> Invitations
    //    {
    //        get
    //        {
    //            return Invitations;
    //        }

            
    //    }

    //    public override DbSet<PaymentTransaction> PaymentTransactions
    //    {
    //        get
    //        {
    //            return PaymentTransactions;
    //        }

    //    }


    //    #endregion

    //    #region Methods
    //    public new void SaveChanges()
    //    {
    //        base.SaveChanges();
    //    }

    //    public override Task<int> SaveChangesAsync()
    //    {
    //        return base.SaveChangesAsync();
    //    }
    //    #endregion
    //}
}
