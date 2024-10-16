using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<CreateUserDTO, User>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

        }
    }
}
