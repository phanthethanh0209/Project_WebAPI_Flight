namespace TheThanh_WebAPI_Flight.Data
{
    public class RoleUser
    {

        public int UserID { get; set; }
        public User Users { get; set; }

        public int RoleID { get; set; }
        public Role Roles { get; set; }

        //public int CreatorID { get; set; }
        //public User Creators { get; set; }
    }
}
