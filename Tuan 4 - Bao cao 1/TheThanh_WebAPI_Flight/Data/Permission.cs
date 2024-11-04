namespace TheThanh_WebAPI_Flight.Data
{
    public class Permission
    {
        public int PermissionID { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
