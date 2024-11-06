namespace TheThanh_WebAPI_Flight.Data
{
    public class DocumentType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }

        // relationship
        public int UserID { get; set; }
        public User Users { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<RoleDocumentType> RoleDocumentTypes { get; set; } = new List<RoleDocumentType>();

    }
}
