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
            CreateMap<Document, DocumentDTO>()/*.ForMember(dest => dest.TypeID, opt => opt.Ignore())*/
                .ForMember(dest => dest.FlightNo, opt => opt.MapFrom(src => src.Flights.FlightNo))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.DocumentTypes.TypeName))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Users.UserName));
        }
    }
}
