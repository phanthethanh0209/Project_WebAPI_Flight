using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingRole : Profile
    {
        public MappingRole()
        {
            CreateMap<RoleDTO, Role>().ReverseMap();
        }
    }
}
