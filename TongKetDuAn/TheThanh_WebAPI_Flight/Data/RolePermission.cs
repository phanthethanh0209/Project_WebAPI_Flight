namespace TheThanh_WebAPI_Flight.Data
{
    public class RolePermission
    {
        public int RoleID { get; set; }
        public Role Roles { get; set; }


        public int PermissionID { get; set; }
        public Permission Permissions { get; set; }


        // relationship RoleDocumentType và RoleDocument
        public ICollection<RoleDocumentType> RoleDocumentTypes { get; set; } = new List<RoleDocumentType>();
        public ICollection<RoleDocument> RoleDocuments { get; set; } = new List<RoleDocument>();


    }
}
