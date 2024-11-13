using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingRoleUser : Profile
    {
        public MappingRoleUser()
        {
            CreateMap<RoleUserDTO, RoleUser>().ReverseMap();
            //.ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.CreatorID));

        }
    }
}
