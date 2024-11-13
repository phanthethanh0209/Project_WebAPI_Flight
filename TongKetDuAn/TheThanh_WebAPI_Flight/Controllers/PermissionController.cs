using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        //[CustomAuthorize("Create")]
        public async Task<IActionResult> CreatePermission(PermissionDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _permissionService.CreatePermission(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _permissionService.GetAllPermission());
        }

        [HttpPut("{PermissionID}")]
        //[CustomAuthorize("Update")]
        public async Task<IActionResult> UpdateRole(int PermissionID, PermissionDTO updateDto)
        {
            (bool Success, string ErrorMessage) result = await _permissionService.UpdatePermission(PermissionID, updateDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _permissionService.GetAllPermission());
        }

        [HttpDelete("{PermissionID}")]
        //[CustomAuthorize("Delete")]
        public async Task<IActionResult> DeleteRole(int PermissionID)
        {
            (bool Success, string ErrorMessage) result = await _permissionService.DeletePermission(PermissionID);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _permissionService.GetAllPermission());
        }

        [HttpGet]
        //[CustomAuthorize("GetAll")]
        public async Task<IActionResult> GetAllRole()
        {
            return Ok(await _permissionService.GetAllPermission());
        }

        [HttpGet("{PermissionID}")]
        //[CustomAuthorize("GetSingle")]
        public async Task<IActionResult> GetByIdRole(int PermissionID)
        {
            PermissionDTO role = await _permissionService.GetPermission(PermissionID);
            if (role == null) return BadRequest("Not found");

            return Ok(role);
        }
    }
}
