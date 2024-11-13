namespace TheThanh_WebAPI_Flight.Data
{
    public class RoleDocumentType
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public RolePermission RolePermissions { get; set; }


        public int TypeID { get; set; }
        public DocumentType DocumentTypes { get; set; }


    }
}
