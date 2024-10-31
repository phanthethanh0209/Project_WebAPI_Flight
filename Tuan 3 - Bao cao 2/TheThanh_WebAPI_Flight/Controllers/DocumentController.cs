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
        public async Task<IActionResult> GetAllDoc(int page = 1)
        {
            return Ok(await _docService.GetAllDocument(page));
        }

        [HttpGet("{DocID}")]
        public async Task<IActionResult> GetByDocID(int docID)
        {
            DocumentDTO doc = await _docService.GetDocByID(docID);
            if (doc == null) return BadRequest("Not found");

            return Ok(doc);
        }

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

        [HttpGet]
        [Route("Download/{DocId}")]
        public async Task<IActionResult> Download(int DocID)
        {
            // Gọi phương thức DownloadFile từ service để lấy dữ liệu file
            (byte[] fileData, string contentType, string originalFileName) = await _docService.DownloadFile(DocID);

            // Trả về file dưới dạng response HTTP
            return File(fileData, contentType, originalFileName);
        }

        //[HttpPut("{DocId}")]
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
