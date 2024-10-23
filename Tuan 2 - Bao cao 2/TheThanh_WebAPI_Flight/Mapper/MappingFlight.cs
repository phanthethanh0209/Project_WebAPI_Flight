using AutoMapper;
using System.Text;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Mapper
{
    public class MappingFlight : Profile
    {
        public MappingFlight()
        {
            CreateMap<CreateFlightDTO, Flight>()
                .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.Date)); // ánh xạ giữa 2 thuộc tính khác nhau của DTO và Enitity
            CreateMap<Flight, FlightDTO>().
                ForMember(dest => dest.Route, opt => opt.MapFrom(src => $"{GetAbbreviation(src.PointLoad)}-{GetAbbreviation(src.PointUnload)}")); // ánh xạ Route  

        }
        private string GetAbbreviation(string location)
        {
            if (string.IsNullOrWhiteSpace(location)) return string.Empty;

            //tách chuỗi thành các từ, dựa trên khoảng trắng(' ')
            //StringSplitOptions.RemoveEmptyEntries giúp loại bỏ những khoảng trắng thừa
            string[] words = location.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder abbreviation = new();

            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    abbreviation.Append(char.ToUpper(word[0]));
                }
            }

            return abbreviation.ToString();
        }

    }
}
