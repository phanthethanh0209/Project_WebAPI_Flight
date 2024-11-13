namespace TheThanh_WebAPI_Flight.Data
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<RoleUser> RoleUsers { get; set; } = new List<RoleUser>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
