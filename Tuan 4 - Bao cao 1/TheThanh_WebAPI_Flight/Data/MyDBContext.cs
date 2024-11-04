using Microsoft.EntityFrameworkCore;

namespace TheThanh_WebAPI_Flight.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<RoleDocumentType> RoleDocumentTypes { get; set; }
        public DbSet<RoleDocument> RoleDocuments { get; set; }

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

                e.HasData(
                    //password = Thanh123@
                    new User { UserID = 1, UserName = "TheThanh29", Email = "thanh@vietjetair.com", Password = "$2a$11$qse3/LUTWo8.qJwnMLI/0eRn.9tIrHP5nn/FUnFfrI7PZSY.yEXRi", Phone = "0147852369" }
                    );
            });

            // Role
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Role");
                e.HasKey(dh => dh.RoleID);
                e.Property(r => r.CreateDate).HasDefaultValueSql("GETDATE()");

                e.HasData(
                    new Role { RoleID = 1, Name = "Admin", Note = "Dành cho quản trị viên" }
                    );
            });

            // Permission
            modelBuilder.Entity<Permission>(e =>
            {
                e.ToTable("Permission");
                e.HasKey(dh => dh.PermissionID);
                e.HasData(
                    new Permission { PermissionID = 1, Name = "Read Only" },
                    new Permission { PermissionID = 2, Name = "Read and modify" },
                    new Permission { PermissionID = 3, Name = "Full permission" }
                    );
            });

            modelBuilder.Entity<RoleUser>(e =>
            {
                e.ToTable("RoleUser");
                e.HasKey(r => new { r.RoleID, r.UserID });

                e.HasOne(r => r.Users)
                    .WithMany(u => u.RoleUsers)
                    .HasForeignKey(r => r.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Roles)
                    .WithMany(role => role.RoleUsers)
                    .HasForeignKey(r => r.RoleID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasData(
                    new RoleUser { RoleID = 1, UserID = 1 } // gán quyền cho admin là full permission
                    );
            });

            // RolePermission
            modelBuilder.Entity<RolePermission>(e =>
            {
                e.ToTable("RolePermission");
                e.HasKey(r => new { r.RoleID, r.PermissionID });

                e.HasOne(r => r.Roles)
                    .WithMany(e => e.RolePermissions)
                    .HasForeignKey(e => e.RoleID);

                e.HasOne(r => r.Permissions)
                    .WithMany(e => e.RolePermissions)
                    .HasForeignKey(e => e.PermissionID);

                e.HasData(
                    new RolePermission { RoleID = 1, PermissionID = 3 } // gán quyền cho admin là full permission
                    );
            });

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.ToTable("RefreshToken");
                e.HasKey(r => new { r.Id });

                e.HasOne(r => r.Users)
                    .WithMany(e => e.RefreshTokens)
                    .HasForeignKey(e => e.UserId);
            });


            //DocumentType
            modelBuilder.Entity<DocumentType>(e =>
            {
                e.ToTable("DocumentType");
                e.HasKey(pk => pk.TypeID);
                e.Property(r => r.CreateDate).HasDefaultValueSql("GETDATE()");

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

                e.HasOne(r => r.Users)
                    .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.UserID)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            // Role Document Type
            modelBuilder.Entity<RoleDocumentType>(e =>
            {
                e.ToTable("RoleDocumentType");
                e.HasKey(r => new { r.RoleID, r.PermissionID, r.TypeID });

                e.HasOne(rdt => rdt.RolePermissions)
                   .WithMany(rp => rp.RoleDocumentTypes)
                   .HasForeignKey(rdt => new { rdt.RoleID, rdt.PermissionID })
                   .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.DocumentTypes)
                    .WithMany(e => e.RoleDocumentTypes)
                    .HasForeignKey(e => e.TypeID);
            });

            // Role Document
            modelBuilder.Entity<RoleDocument>(e =>
            {
                e.ToTable("RoleDocument");
                e.HasKey(r => new { r.RoleID, r.PermissionID, r.DocID });

                e.HasOne(rdt => rdt.RolePermissions)
                   .WithMany(rp => rp.RoleDocuments)
                   .HasForeignKey(rdt => new { rdt.RoleID, rdt.PermissionID })
                   .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Documents)
                    .WithMany(e => e.RoleDocuments)
                    .HasForeignKey(e => e.DocID);
            });
        }
    }
}
