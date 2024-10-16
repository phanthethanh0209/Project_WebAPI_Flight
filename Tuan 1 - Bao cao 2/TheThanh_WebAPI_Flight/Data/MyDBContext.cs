using Microsoft.EntityFrameworkCore;

namespace TheThanh_WebAPI_Flight.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(pk => pk.UserID);
                e.HasIndex(e => e.Email).IsUnique();
            });

            //DocumentType
            modelBuilder.Entity<DocumentType>(e =>
            {
                e.ToTable("DocumentType");
                e.HasKey(pk => pk.TypeID);
            });
        }
    }
}
