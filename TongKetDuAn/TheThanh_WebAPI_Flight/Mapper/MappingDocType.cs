using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingDocType : Profile
    {
        public MappingDocType()
        {
            CreateMap<CreateDocTypeDTO, DocumentType>().ReverseMap();
            CreateMap<DocumentType, ListDocTypeDTO>();
        }
    }
}
