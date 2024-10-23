using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _docService;

        public DocumentController(IDocumentService docService)
        {
            _docService = docService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoc()
        {
            return Ok(await _docService.GetAllDocument());
        }

        //[HttpGet("{FlightNo}")]
        //public async Task<IActionResult> GetByDocID(string docID)
        //{
        //    FlightDTO flight = await _docService.GetFLight(docID);
        //    if (flight == null) return BadRequest("Not found");

        //    return Ok(flight);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _docService.CreateDocument(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _docService.GetAllDocument());
        }

        //[HttpPost]
        //public async Task<IActionResult> Download( fileName)
        //{
        //    // Gọi phương thức DownloadFile từ service để lấy dữ liệu file
        //    var (fileData, contentType, originalFileName) = await _docService.DownloadFile(fileName);

        //    // Trả về file dưới dạng response HTTP
        //    return File(fileData, contentType, originalFileName);
        //}

        //[HttpPut("{FlightID}")]
        //public async Task<IActionResult> UpdateUser(int flightID, CreateFlightDTO updateDto)
        //{
        //    (bool Success, string ErrorMessage) result = await _flightService.UpdateFlight(flightID, updateDto);

        //    if (!result.Success)
        //    {
        //        return BadRequest(result.ErrorMessage);
        //    }

        //    return Ok(await _flightService.GetAllFLight());
        //}

        [HttpDelete("{DocID}")]
        public async Task<IActionResult> DeleteDocument(int docID)
        {
            (bool Success, string ErrorMessage) result = await _docService.DeleteDocument(docID);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _docService.GetAllDocument());
        }
    }
}
