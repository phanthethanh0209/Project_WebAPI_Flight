namespace TheThanh_WebAPI_Flight.Data
{
    public class Flight
    {
        public int FlightID { get; set; }
        public string FlightNo { get; set; }
        public DateTime DepartureDate { get; set; }
        public string PointLoad { get; set; }
        public string PointUnload { get; set; }
        public int TotalDoc { get; set; }


        // relationship
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
