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
        public ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();

    }
}
