namespace TheThanh_WebAPI_Flight.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User Users { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IsssueAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
