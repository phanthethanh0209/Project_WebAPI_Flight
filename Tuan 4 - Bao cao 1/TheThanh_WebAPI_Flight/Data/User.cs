namespace TheThanh_WebAPI_Flight.Data
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

        // relationship
        public ICollection<RoleUser> RoleUsers { get; set; } = new List<RoleUser>();
        //public ICollection<RoleUser> CreatedRoles { get; set; } = new List<RoleUser>(); // người tạo role
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
