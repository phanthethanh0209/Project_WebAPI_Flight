using Microsoft.EntityFrameworkCore.Storage;
using TheThanh_WebAPI_Flight.Data;

namespace TheThanh_WebAPI_Flight.Repository
{
    public interface IRepositoryWrapper
    {
        IRepositoryBase<User> User { get; }
        IRepositoryBase<Role> Role { get; }
        IRepositoryBase<Permission> Permission { get; }
        IRepositoryBase<RolePermission> RolePermission { get; }
        IRepositoryBase<RoleUser> RoleUser { get; }
        IRepositoryBase<RefreshToken> RefreshToken { get; }
        IRepositoryBase<DocumentType> DocumentType { get; }
        IRepositoryBase<Flight> Flight { get; }
        IRepositoryBase<Document> Document { get; }


        Task SaveChangeAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MyDBContext _db;

        public RepositoryWrapper(MyDBContext db)
        {
            _db = db;
        }

        private IRepositoryBase<User> UserRepositoryBase;
        public IRepositoryBase<User> User => UserRepositoryBase ??= new RepositoryBase<User>(_db);


        public IRepositoryBase<Role> RoleRepositoryBase;
        public IRepositoryBase<Role> Role => RoleRepositoryBase ??= new RepositoryBase<Role>(_db);


        public IRepositoryBase<Permission> PermissionRepositoryBase;
        public IRepositoryBase<Permission> Permission => PermissionRepositoryBase ??= new RepositoryBase<Permission>(_db);


        public IRepositoryBase<RolePermission> RolePermissionRepositoryBase;
        public IRepositoryBase<RolePermission> RolePermission => RolePermissionRepositoryBase ??= new RepositoryBase<RolePermission>(_db);


        public IRepositoryBase<RoleUser> RoleUserRepositoryBase;
        public IRepositoryBase<RoleUser> RoleUser => RoleUserRepositoryBase ??= new RepositoryBase<RoleUser>(_db);


        public IRepositoryBase<RefreshToken> RefreshTokenRepositoryBase;
        public IRepositoryBase<RefreshToken> RefreshToken => RefreshTokenRepositoryBase ??= new RepositoryBase<RefreshToken>(_db);



        private IRepositoryBase<DocumentType> DocumentTypeRepositoryBase;
        public IRepositoryBase<DocumentType> DocumentType => DocumentTypeRepositoryBase ??= new RepositoryBase<DocumentType>(_db);


        private IRepositoryBase<Flight> FlightRepositoryBase;
        public IRepositoryBase<Flight> Flight => FlightRepositoryBase ??= new RepositoryBase<Flight>(_db);


        private IRepositoryBase<Document> DocumentRepositoryBase;
        public IRepositoryBase<Document> Document => DocumentRepositoryBase ??= new RepositoryBase<Document>(_db);


        public async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }
}
