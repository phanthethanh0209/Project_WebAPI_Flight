namespace TheThanh_WebAPI_Flight.Data
{
    public class Document
    {
        public int DocID { get; set; }
        public string DocName { get; set; }
        //public string File { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }

        // relationship
        public int UserID { get; set; }
        public User Users { get; set; }

        public int TypeID { get; set; }
        public DocumentType DocumentTypes { get; set; }

        public int FlightID { get; set; }
        public Flight Flights { get; set; }


        public ICollection<RoleDocument> RoleDocuments { get; set; } = new List<RoleDocument>();
        // còn version
    }
}
