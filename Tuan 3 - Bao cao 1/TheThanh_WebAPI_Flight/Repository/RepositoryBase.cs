using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheThanh_WebAPI_Flight.Data;

namespace TheThanh_WebAPI_Flight.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> expression = null, int pageNumber = 1, int pageSize = 3, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression = null);
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly MyDBContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(MyDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        //public async Task<IEnumerable<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> expression = null, int pageNumber = 1, int pageSize = 3)
        //{
        //    IQueryable<T> query = _dbSet;

        //    // Kiểm tra điều kiện lọc
        //    if (expression != null)
        //    {
        //        query = query.Where(expression);
        //    }

        //    // Sắp xếp giảm dần theo Id hoặc khóa chính
        //    query = query.OrderByDescending(e => e); // bạn có thể thay đổi theo yêu cầu

        //    // phân trang
        //    query = query
        //        .Skip((pageNumber - 1) * pageSize) // bỏ qua số lượng phần tử tính từ đầu
        //        .Take(pageSize); // lấy số lượng phần tử tiếp theo

        //    // Trả về kết quả phân trang
        //    return await query.ToListAsync();
        //}

        public async Task<IEnumerable<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> expression = null, int pageNumber = 1, int pageSize = 3, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Kiểm tra điều kiện lọc
            if (expression != null)
            {
                query = query.Where(expression);
            }

            // Thêm các bảng liên quan
            if (includes != null)
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Sắp xếp giảm dần theo Id hoặc khóa chính
            query = query.OrderByDescending(e => e); // bạn có thể thay đổi theo yêu cầu

            // phân trang
            query = query
                .Skip((pageNumber - 1) * pageSize) // bỏ qua số lượng phần tử tính từ đầu
                .Take(pageSize); // lấy số lượng phần tử tiếp theo

            // Trả về kết quả phân trang
            return await query.ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            IQueryable<T> query = _dbSet;

            // Kiểm tra điều kiện lọc
            if (expression != null)
            {
                query = query.Where(expression);
            }

            // Sắp xếp giảm dần khóa chính
            query = query.OrderByDescending(e => e);

            // Trả về dữ liệu không phân trang
            return await query.ToListAsync();
        }


        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Kiểm tra điều kiện lọc
            if (expression != null)
            {
                query = query.Where(expression);
            }

            // Thêm các bảng liên quan
            if (includes != null)
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _dbSet.AnyAsync();
            }
            else
            {
                return await _dbSet.AnyAsync(expression);
            }
        }
    }
}
