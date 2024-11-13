using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;
using static TheThanh_WebAPI_Flight.Authorization.CustomAuthorizationAttribute;

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

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet]
        public async Task<IActionResult> GetAllDoc(int page = 1)
        {
            return Ok(await _docService.GetAllDocument(page));
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet("search")]
        public async Task<IActionResult> GetAllDocByName(string docName, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(docName))
            {
                return BadRequest("Document name cannot be empty.");
            }
            return Ok(await _docService.GetAllDocByName(docName, page));
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet("{DocID}")]
        public async Task<IActionResult> GetByDocID(int docID)
        {
            DocumentDTO doc = await _docService.GetDocByID(docID);
            if (doc == null) return BadRequest("Not found");

            return Ok(doc);
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet("type")]
        public async Task<IActionResult> GetAllDocByTypeID(int typeId, int page = 1)
        {
            return Ok(await _docService.GetAllDocByTypeID(typeId, page));
        }

        [CustomAuthorize(new[] { "Full permission" })]
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

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet]
        [Route("Download/{DocId}")]
        public async Task<IActionResult> Download(int DocID)
        {
            // Gọi phương thức DownloadFile từ service để lấy dữ liệu file
            (byte[] fileData, string contentType, string originalFileName) = await _docService.DownloadFile(DocID);

            // Trả về file dưới dạng response HTTP
            return File(fileData, contentType, originalFileName);
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet]
        [Route("Download/Flight/{FlightID}")]
        public async Task<IActionResult> DownloadFilesByFlightID(int FlightId)
        {
            // Gọi phương thức DownloadFile từ service để lấy dữ liệu file
            (byte[] fileData, string contentType, string originalFileName) = await _docService.DownloadFilesByFlightID(FlightId);

            // Trả về file dưới dạng response HTTP
            return File(fileData, contentType, originalFileName);
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet]
        [Route("Download/search")]
        public async Task<IActionResult> DownloadFilesByDocName(string docName, int page = 1)
        {
            // Gọi phương thức DownloadFile từ service để lấy dữ liệu file
            (byte[] fileData, string contentType, string originalFileName) = await _docService.DownloadFilesByDocName(docName, page);
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

        [CustomAuthorize(new[] { "Full permission" })]
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
