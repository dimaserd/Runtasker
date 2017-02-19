using Runtasker.Logic.Entities;
using System.Data.Entity;

namespace Runtasker.Logic.Contexts.Interfaces
{
    public interface IMyDbContext
    {
        #region From IApplicationDbContext
        //DbSet<ApplicationUser> Users { get; set; }

        //DbSet<IdentityRole> Roles { get; set; }

        #endregion

        IDbSet<Order> Orders { get; set; }

        IDbSet<Message> Messages { get; set; }

        IDbSet<Attachment> Attachments { get; set; }

        IDbSet<Payment> Payments { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        IDbSet<Invitation> Invitations { get; set; }

        IDbSet<PaymentTransaction> PaymentTransactions { get; set; }

        #region VkParse
        //IDbSet<VkGroup> VkGroups { get; set; }

        //IDbSet<VkFoundPost> VkFoundPosts { get; set; }

        //IDbSet<VkKeyWord> VkKeyWords { get; set; }

        //IDbSet<VkPostLookUp> VkPostLookUps { get; set; }

        #endregion
    }
}
