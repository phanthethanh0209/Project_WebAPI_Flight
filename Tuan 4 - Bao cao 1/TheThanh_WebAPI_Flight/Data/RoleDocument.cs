namespace TheThanh_WebAPI_Flight.Data
{
    public class RoleDocument
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public RolePermission RolePermissions { get; set; }


        public int DocID { get; set; }
        public Document Documents { get; set; }
    }
}
