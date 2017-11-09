using VkParser.Entities;
using Runtasker.Settings.Statics;
using System.Data.Entity;
using VkParser.Entities.Spam;

namespace VkParser.Contexts
{
    public class VkParseContext : DbContext
    {
        public VkParseContext() : base(ConnectionStringStatic.ConnectionString)
        {

        }
        #region DbSets
        public DbSet<VkGroup> VkGroups { get; set; }

        public DbSet<VkFoundPost> VkFoundPosts { get; set; }

        public DbSet<VkKeyWord> VkKeyWords { get; set; }

        public DbSet<VkPostLookUp> VkPostLookUps { get; set; }

        #region Спамирование

        public DbSet<VkGroupMember> VkGroupMembers { get; set; }

        public DbSet<VkMan> VkMans { get; set; }

        
        #endregion

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VkGroupMember>().HasKey(e => new { e.VkManId, e.VkGroupId });

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
