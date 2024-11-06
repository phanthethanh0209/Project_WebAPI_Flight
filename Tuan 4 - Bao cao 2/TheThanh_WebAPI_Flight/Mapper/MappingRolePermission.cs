using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingRolePermission : Profile
    {
        public MappingRolePermission()
        {
            CreateMap<RolePermissionDTO, RolePermission>().ReverseMap();
        }
    }
}
