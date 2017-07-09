using Runtasker.Logic.Contexts;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using VkParser.Entities;
using System;

namespace Runtasker.Logic
{
    public class MyDbContext : ApplicationDbContext, IMyDbContext
    {
        #region Constructors
        public MyDbContext() : base()
        {

        }

        public static MyDbContext CreateContextForTesting()
        {
            return new MyDbContext("LocalTestConnection");
        }
        

        public MyDbContext(string connection) : base(connection)
        {
            
        }
        #endregion

        public DbSet<Coupon> Coupons { get; set; }
        
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Таблица содержащая решения заказов
        /// </summary>
        //public DbSet<OrderSolution> OrderSolutions { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        
       

        #region VkParse
        public DbSet<VkGroup> VkGroups { get; set; }

        public DbSet<VkFoundPost> VkFoundPosts { get; set; }

        public DbSet<VkKeyWord> VkKeyWords { get; set; }

        public DbSet<VkPostLookUp> VkPostLookUps { get; set; }

        
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VkKeyWord>()
                .HasMany(t => t.VkFoundPosts)
                .WithMany(t => t.VkKeyWords)
                .Map(m =>
                {
                    m.ToTable("VkKeyWordFoundPosts");
                    m.MapLeftKey("VkKeyWordId");
                    m.MapRightKey("VkFoundPostId");
                });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(t => t.Coupons)
                .WithMany(t => t.Users)
                .Map(m =>
                {
                    m.ToTable("UserCoupons");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("CouponId");
                });


            base.OnModelCreating(modelBuilder);
        }

        void IMyDbContext.SaveChanges()
        {
            base.SaveChanges();
        }
    }


}
