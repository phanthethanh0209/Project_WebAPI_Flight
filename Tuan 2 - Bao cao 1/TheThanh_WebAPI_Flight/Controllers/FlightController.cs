using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _flightService.GetAllFLight());
        }

        [HttpGet("{FlightNo}")]
        public async Task<IActionResult> GetByFlightNo(string flightNo)
        {
            FlightDTO flight = await _flightService.GetFLight(flightNo);
            if (flight == null) return BadRequest("Not found");

            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight(CreateFlightDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _flightService.CreateFlight(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _flightService.GetAllFLight());
        }

        [HttpPut("{FlightID}")]
        public async Task<IActionResult> UpdateUser(int flightID, CreateFlightDTO updateDto)
        {
            (bool Success, string ErrorMessage) result = await _flightService.UpdateFlight(flightID, updateDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _flightService.GetAllFLight());
        }

        [HttpDelete("{FlightID}")]
        public async Task<IActionResult> DeleteUser(int flightID)
        {
            (bool Success, string ErrorMessage) result = await _flightService.DeleteFlight(flightID);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _flightService.GetAllFLight());


        }
    }
}
