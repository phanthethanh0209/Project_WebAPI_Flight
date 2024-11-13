namespace TheThanh_WebAPI_Flight.DTO
{
    public class FlightResponse
    {
        public int FlightID { get; set; }
        public string FlightNo { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Route { get; set; }
        public List<DocumentDTO>? DocumentDTOs { get; set; }
    }
}
