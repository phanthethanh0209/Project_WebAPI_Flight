using Microsoft.EntityFrameworkCore.Storage;
using TheThanh_WebAPI_Flight.Data;

namespace TheThanh_WebAPI_Flight.Repository
{
    public interface IRepositoryWrapper
    {
        IRepositoryBase<User> User { get; }
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
