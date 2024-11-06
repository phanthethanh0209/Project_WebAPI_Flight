using Microsoft.EntityFrameworkCore;
using TheThanh_WebAPI_Flight.Data;

namespace TheThanh_WebAPI_Flight.Authorization
{
    public interface IUserPermission
    {
        //Task<List<Permission>> GetPermissionsAsync(int userId);
        Task<List<Permission>> GetPermissionsAsync(int userId, int? typeId, int? documentId);
    }

    public class UserPermission : IUserPermission
    {
        private readonly MyDBContext _db;

        public UserPermission(MyDBContext db)
        {
            _db = db;
        }

        public async Task<List<Permission>> GetPermissionsAsync(int userId, int? typeId = null, int? documentId = null)
        {
            List<int> userRoles = await _db.RoleUsers
                .Where(ru => ru.UserID == userId)
                .Select(ru => ru.RoleID)
                .ToListAsync();

            IQueryable<Permission> permissionsQuery;

            if (documentId.HasValue)
            {
                // Lấy quyền từ bảng RoleDocument khi có DocumentID
                permissionsQuery = _db.RoleDocuments
                    .Where(rd => userRoles.Contains(rd.RoleID) && rd.DocID == documentId)
                    .Select(rd => rd.RolePermissions.Permissions);
            }
            else if (typeId.HasValue)
            {
                // Lấy quyền từ bảng RoleDocumentType khi không có DocumentID nhưng có TypeID
                permissionsQuery = _db.RoleDocumentTypes
                    .Where(rdt => userRoles.Contains(rdt.RoleID) && rdt.TypeID == typeId)
                    .Select(rdt => rdt.RolePermissions.Permissions);
            }
            else
            {
                //return new List<Permission>();
                permissionsQuery = _db.RolePermissions
                    .Where(rp => rp.Roles.RoleUsers.Any(ru => ru.UserID == userId))
                    .Select(rp => rp.Permissions);
            }

            return await permissionsQuery.ToListAsync();
        }

    }

}
