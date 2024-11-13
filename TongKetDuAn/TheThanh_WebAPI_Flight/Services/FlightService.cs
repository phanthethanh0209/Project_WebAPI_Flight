using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IFlightService
    {
        //Task<IEnumerable<FlightDTO>> GetAllFLight(int page = 1);
        Task<IEnumerable<FlightResponse>> GetAllFLight(int page = 1);
        Task<FlightDTO> GetFLight(string name);
        Task<(bool Success, string ErrorMessage)> CreateFlight(CreateFlightDTO createDTO);
        Task<(bool Success, string ErrorMessage)> UpdateFlight(int id, CreateFlightDTO updateFlightDTO);
        Task<(bool Success, string ErrorMessage)> DeleteFlight(int id);

    }
    public class FlightService : IFlightService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        //public static int PAGE_SIZE { get; set; } = 3;

        public FlightService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateFlight(CreateFlightDTO createDTO)
        {
            //FluentValidation.Results.ValidationResult validationResult = await _createValidator.ValidateAsync(createDTO);

            //if (!validationResult.IsValid)
            //    return (false, validationResult.Errors.First().ErrorMessage);
            string flightNo = await GenerateFlightNo();

            Flight newFlight = _mapper.Map<Flight>(createDTO);
            newFlight.FlightNo = flightNo;

            await _repository.Flight.CreateAsync(newFlight);
            return (true, null);
        }

        // Phương thức sinh mã FlightNo
        private async Task<string> GenerateFlightNo()
        {
            string newFlightNo = "VJ";
            // Lấy FlightNo lớn nhất
            Flight? lastFlight = (await _repository.Flight.GetAllAsync()).FirstOrDefault();

            if ((lastFlight == null))
            {
                newFlightNo += "001";
            }
            else
            {
                string lastNumberString = lastFlight.FlightNo.Substring(2);
                int num = int.Parse(lastNumberString) + 1;
                if (num < 10)
                    newFlightNo += "00" + num;
                else
                {
                    if (num < 100)
                        newFlightNo += "0" + num;
                    else
                        newFlightNo += num;
                }
            }

            return newFlightNo;
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateFlight(int flightID, CreateFlightDTO updateFlightDTO)
        {
            Flight flight = await _repository.Flight.GetByIdAsync(m => m.FlightID == flightID);
            if (flight == null)
            {
                return (false, "Flight not found");
            }

            _mapper.Map(updateFlightDTO, flight);
            await _repository.Flight.UpdateAsync(flight);
            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteFlight(int id)
        {
            Flight flight = await _repository.Flight.GetByIdAsync(u => u.FlightID == id);
            if (flight == null)
                return (false, "Flight not found.");

            await _repository.Flight.DeleteAsync(flight);
            return (true, null);
        }

        //public async Task<IEnumerable<FlightResponse>> GetAllFLight(int pageNumber)
        //{
        //    IEnumerable<Flight> flights = await _repository.Flight
        //        .GetAllWithPaginationAsync(null, pageNumber, 3, f => f.Documents);

        //    // map Flight với ListFlightResponse
        //    List<FlightResponse> flightResponses = _mapper.Map<List<FlightResponse>>(flights);

        //    return flightResponses;
        //}

        public async Task<IEnumerable<FlightResponse>> GetAllFLight(int pageNumber)
        {
            // Lấy danh sách Flight cùng với Documents
            IEnumerable<Flight> flights = await _repository.Flight
                .GetAllWithPaginationAsync(null, pageNumber, 3, f => f.Documents);

            // Lấy tất cả các TypeID, userID trong từng document 
            List<int> typeIds = flights.SelectMany(f => f.Documents)
                                        .Select(d => d.TypeID)
                                        .Distinct()
                                        .ToList();

            List<int> userIds = flights.SelectMany(f => f.Documents)
                                        .Select(d => d.UserID)
                                        .Distinct()
                                        .ToList();

            // Lấy DocumentTypes cho list TypeID trên
            IEnumerable<DocumentType> documentTypes = await _repository.DocumentType
                .GetAllAsync(dt => typeIds.Contains(dt.TypeID));

            IEnumerable<User> users = await _repository.User
                .GetAllAsync(dt => userIds.Contains(dt.UserID));

            // Chuyển đổi DocumentTypes để dễ dàng tra cứu
            Dictionary<int, string> documentTypeDictionary = documentTypes.ToDictionary(dt => dt.TypeID, dt => dt.TypeName);
            Dictionary<int, string> userDictionary = users.ToDictionary(dt => dt.UserID, dt => dt.UserName);

            // Ánh xạ dữ liệu từ flights sang FlightResponse và gán TypeName cho mỗi Document
            List<FlightResponse> flightResponses = _mapper.Map<List<FlightResponse>>(flights);
            foreach (FlightResponse flightResponse in flightResponses)
            {
                foreach (DocumentDTO document in flightResponse.DocumentDTOs)
                {
                    // Gán TypeName từ documentTypeDictionary
                    if (documentTypeDictionary.TryGetValue(document.TypeID, out string typeName))
                    {
                        document.TypeName = typeName;
                    }
                    if (userDictionary.TryGetValue(document.TypeID, out string userName))
                    {
                        document.Creator = userName;
                    }
                }
            }

            return flightResponses;
        }



        public async Task<FlightDTO> GetFLight(string flightNo)
        {
            Flight flight = await _repository.Flight.GetByIdAsync(x => x.FlightNo == flightNo);
            if (flight == null)
            {
                return null;
            }
            return _mapper.Map<FlightDTO>(flight);
        }

    }
}
