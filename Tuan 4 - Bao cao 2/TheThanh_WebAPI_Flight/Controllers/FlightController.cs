using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;
using static TheThanh_WebAPI_Flight.Authorization.CustomAuthorizationAttribute;

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

        [CustomAuthorize(new[] { "Full permission" })]
        [HttpGet]
        public async Task<IActionResult> GetAllFLight(int page = 1)
        {
            return Ok(await _flightService.GetAllFLight(page));
        }

        [CustomAuthorize(new[] { "Full permission" })]
        [HttpGet("{FlightNo}")]
        public async Task<IActionResult> GetByFlightNo(string flightNo)
        {
            FlightDTO flight = await _flightService.GetFLight(flightNo);
            if (flight == null) return BadRequest("Not found");

            return Ok(flight);
        }

        [CustomAuthorize(new[] { "Full permission" })]
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

        [CustomAuthorize(new[] { "Full permission" })]
        [HttpPut("{FlightID}")]
        public async Task<IActionResult> UpdateFlight(int flightID, CreateFlightDTO updateDto)
        {
            (bool Success, string ErrorMessage) result = await _flightService.UpdateFlight(flightID, updateDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _flightService.GetAllFLight());
        }

        [CustomAuthorize(new[] { "Full permission" })]
        [HttpDelete("{FlightID}")]
        public async Task<IActionResult> DeleteFlight(int flightID)
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
