namespace TheThanh_WebAPI_Flight.DTO
{
    public class CreateDocTypeDTO
    {
        public string TypeName { get; set; }
        public string Note { get; set; }
        public int UserID { get; set; } // đang giả định user sẽ bỏ sau khi đưa jwt
    }
}
