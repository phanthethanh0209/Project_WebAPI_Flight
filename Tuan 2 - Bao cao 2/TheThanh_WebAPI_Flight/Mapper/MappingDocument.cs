using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingDocument : Profile
    {
        public MappingDocument()
        {
            CreateMap<CreateDocumentDTO, Document>();
            CreateMap<Document, DocumentDTO>();
        }
    }
}
