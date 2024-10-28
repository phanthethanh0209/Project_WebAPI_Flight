using Microsoft.EntityFrameworkCore;

namespace TheThanh_WebAPI_Flight.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Flight> Flights { get; set; }
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

                e.HasOne(r => r.Users)
                    .WithMany(e => e.DocumentTypes)
                    .HasForeignKey(e => e.UserID);
            });

            //Flight
            modelBuilder.Entity<Flight>(e =>
            {
                e.ToTable("Flight");
                e.HasKey(pk => pk.FlightID);
                e.HasIndex(e => e.FlightNo).IsUnique();
            });

            //Document
            modelBuilder.Entity<Document>(e =>
            {
                e.ToTable("Document");
                e.HasKey(pk => pk.DocID);
                e.Property(r => r.CreateDate).HasDefaultValueSql("GETDATE()");

                e.HasOne(r => r.DocumentTypes)
                    .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.TypeID);

                e.HasOne(r => r.Flights)
                    .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.FlightID);
            });
        }
    }
}
