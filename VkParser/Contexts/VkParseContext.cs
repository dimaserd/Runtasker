using VkParser.Entities;
using Runtasker.Settings.Statics;
using System.Data.Entity;

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
