using Microsoft.AspNetCore.Authorization;

namespace TheThanh_WebAPI_Flight.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] Permission { get; set; }
        public int? TypeID { get; set; } // Sẽ được thiết lập từ `PermissionFilter`
        public int? DocumentID { get; set; } // Sẽ được thiết lập từ `PermissionFilter`

        public PermissionRequirement(string[] permission)
        {
            Permission = permission;
        }
    }
}
