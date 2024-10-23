using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDTO>> GetAllFLight(int page = 1);
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

        public async Task<IEnumerable<FlightDTO>> GetAllFLight(int pageNumber)
        {
            IEnumerable<Flight> flights = await _repository.Flight.GetAllWithPaginationAsync(null, pageNumber);
            return _mapper.Map<IEnumerable<FlightDTO>>(flights);
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
