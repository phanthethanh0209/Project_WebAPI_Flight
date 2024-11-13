using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingPermission : Profile
    {
        public MappingPermission()
        {
            CreateMap<PermissionDTO, Permission>().ReverseMap();
        }
    }
}
