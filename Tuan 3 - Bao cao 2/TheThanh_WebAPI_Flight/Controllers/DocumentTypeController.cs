using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;

        public DocumentTypeController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocumentType(int page = 1)
        {
            return Ok(await _documentTypeService.GetAllDocumentType(page));
        }

        [HttpGet("{TypeId}")]
        public async Task<IActionResult> GetDocumentType(int typeId)
        {
            CreateDocTypeDTO type = await _documentTypeService.GetDocumentType(typeId);
            if (type == null) return BadRequest("Not found");

            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocType(CreateDocTypeDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _documentTypeService.CreateDocumentType(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _documentTypeService.GetAllDocumentType());
        }

        [HttpPut("{TypeId}")]
        public async Task<IActionResult> UpdateDocType(int typeId, CreateDocTypeDTO updateDto)
        {
            (bool Success, string ErrorMessage) result = await _documentTypeService.UpdateDocumentType(typeId, updateDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _documentTypeService.GetAllDocumentType());
        }

        [HttpDelete("{TypeId}")]
        public async Task<IActionResult> DeleteDocType(int typeId)
        {
            (bool Success, string ErrorMessage) result = await _documentTypeService.DeleteDocumentType(typeId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _documentTypeService.GetAllDocumentType());
        }
    }
}
