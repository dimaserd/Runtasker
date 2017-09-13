using Runtasker.Logic.Contexts;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using VkParser.Entities;
using System;
using Runtasker.Logic.Entities.News;
using Runtasker.Logic.Entities.ClickLinks;

namespace Runtasker.Logic
{
    public class MyDbContext : ApplicationDbContext, IMyDbContext
    {
        #region Конструкторы
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

        #region Свойства - Таблицы

        #region Новости
        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleClarification> ArticleClarifications { get; set; }
        #endregion

        #region Вопросы-Ответы
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public DbSet<QuestionAnswerLangClarification> QuestionAnswerLangClarifications { get; set; }
        #endregion

        #region Клики
        public DbSet<CountingClickLink> CountingClickLinks { get; set; }

        public DbSet<Click> Clicks { get; set; }
        #endregion


        public DbSet<Coupon> Coupons { get; set; }
        
        public DbSet<Order> Orders { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        #region Оплата
        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        #endregion

        #region Таблица VkParse
        public DbSet<VkGroup> VkGroups { get; set; }

        public DbSet<VkFoundPost> VkFoundPosts { get; set; }

        public DbSet<VkKeyWord> VkKeyWords { get; set; }

        public DbSet<VkPostLookUp> VkPostLookUps { get; set; }


        #endregion

        #endregion

        #region Методы

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
        #endregion

    }


}
