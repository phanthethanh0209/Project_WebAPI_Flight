using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TheThanh_WebAPI_Flight.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserPermission _permissionService;

        public PermissionHandler(IUserPermission permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Lấy userId từ claim
            Claim? userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return;
            }

            // ktra quyền admin
            List<Data.Permission> adminPermissions = await _permissionService.GetPermissionsAsync(userId, null, null);
            if (adminPermissions.Any(p => p.Name == "Full permission"))
            {
                context.Succeed(requirement); // Cấp quyền truy cập tất cả tài liệu
                return;
            }

            // Gọi UserPermission để lấy danh sách quyền của user cho loại tài liệu và tài liệu được yêu cầu
            List<Data.Permission> userPermissions = await _permissionService.GetPermissionsAsync(userId, requirement.TypeID, requirement.DocumentID);

            // Kiểm tra nếu user có quyền
            if (userPermissions != null && userPermissions.Any(p => requirement.Permission.Contains(p.Name)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

    }
}
