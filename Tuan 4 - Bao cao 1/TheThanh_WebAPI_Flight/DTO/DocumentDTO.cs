namespace TheThanh_WebAPI_Flight.DTO
{
    public class DocumentDTO
    {
        public string DocName { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }

        // còn thiếu creator
        //public int FlightID { get; set; }
        public string FlightNo { get; set; }

    }
}
