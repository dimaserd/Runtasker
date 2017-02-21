using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Interfaces
{
    public interface IMyDbContext
    {
        
        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<OtherUserInfo> OtherUserInfos { get; set; }

        

        IDbSet<Order> Orders { get; set; }

        IDbSet<Message> Messages { get; set; }

        IDbSet<Attachment> Attachments { get; set; }

        IDbSet<Payment> Payments { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        IDbSet<Invitation> Invitations { get; set; }

        IDbSet<PaymentTransaction> PaymentTransactions { get; set; }

        void SaveChanges();

        Task<int> SaveChangesAsync();
        
    }
}
