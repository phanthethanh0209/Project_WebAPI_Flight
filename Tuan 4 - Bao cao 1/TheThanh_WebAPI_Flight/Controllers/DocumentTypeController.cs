using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;
using static TheThanh_WebAPI_Flight.Authorization.CustomAuthorizationAttribute;

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

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet]
        public async Task<IActionResult> GetAllDocumentType(int page = 1)
        {
            return Ok(await _documentTypeService.GetAllDocumentType(page));
        }

        [CustomAuthorize(new[] { "Read Only", "Read and modify", "Full permission" })]
        [HttpGet("{TypeId}")]
        public async Task<IActionResult> GetDocumentType(int typeId)
        {
            CreateDocTypeDTO type = await _documentTypeService.GetDocumentType(typeId);
            if (type == null) return BadRequest("Not found");

            return Ok(type);
        }

        [CustomAuthorize(new[] { "Full permission" })]
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

        [CustomAuthorize(new[] { "Read and modify", "Full permission" })]
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

        [CustomAuthorize(new[] { "Full permission" })]
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
