﻿namespace TheThanh_WebAPI_Flight.DTO
{
    public class CreateDocumentDTO
    {
        public IFormFile DocName { get; set; }
        public int TypeID { get; set; }
        public int FlightID { get; set; }
        public string Note { get; set; }


        // Còn thiếu version cho tài liệu (1 tài liệu có nhiều version)
        // Còn thiếu Add group cho permission

    }
}
