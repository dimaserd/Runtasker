﻿using Runtasker.Logic.Contexts;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading.Tasks;
using VkParser.Entities;

namespace Runtasker.Logic
{
    public class MyDbContext : ApplicationDbContext
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

        public DbSet<OtherUserInfo> OtherUserInfos { get; set; }

        public DbSet<Order> Orders { get; set; }

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
            base.OnModelCreating(modelBuilder);
        }

    }


}
