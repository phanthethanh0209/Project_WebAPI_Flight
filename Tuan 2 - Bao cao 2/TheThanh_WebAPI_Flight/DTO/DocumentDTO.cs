namespace TheThanh_WebAPI_Flight.DTO
{
    public class DocumentDTO
    {
        public string DocName { get; set; }
        public int TypeID { get; set; }
        public DateTime CreateDate { get; set; }

        // còn thiếu creator
        public int FlightID { get; set; }
    }
}
