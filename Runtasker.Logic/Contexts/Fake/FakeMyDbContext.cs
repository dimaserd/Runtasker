using FakeDbSet;
using Runtasker.Logic.Entities;
using VkParser.Entities;

namespace Runtasker.Logic.Contexts.Fake
{
    public class FakeMyDbContext //: IMyDbContext
    {
        #region From IApplicationDbContext
        //InMemoryDbSet<ApplicationUser> Users { get; set; }

        //InMemoryDbSet<IdentityRole> Roles { get; set; }

        #endregion

        //IDbSet<Order> Orders
        //{
        //    get
        //    {
        //        return new InMemoryDbSet<Order>();
        //    }
        //    set { value }

        //}

        InMemoryDbSet<Message> Messages { get; set; }

        InMemoryDbSet<Attachment> Attachments { get; set; }

        InMemoryDbSet<Payment> Payments { get; set; }

        InMemoryDbSet<Notification> Notifications { get; set; }

        InMemoryDbSet<Invitation> Invitations { get; set; }

        InMemoryDbSet<PaymentTransaction> PaymentTransactions { get; set; }

        #region VkParse
        InMemoryDbSet<VkGroup> VkGroups { get; set; }

        InMemoryDbSet<VkFoundPost> VkFoundPosts { get; set; }

        InMemoryDbSet<VkKeyWord> VkKeyWords { get; set; }

        InMemoryDbSet<VkPostLookUp> VkPostLookUps { get; set; }

        #endregion
    }
}
