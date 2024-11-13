using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {

        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpPost]
        //[CustomAuthorize("Create")]
        public async Task<IActionResult> AddPerrmissionToRole(RolePermissionDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _rolePermissionService.AddPermissionToRole(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _rolePermissionService.GetAllRolePermission());
        }

        [HttpDelete("{RoleID}/{PermissionID}")]
        //[CustomAuthorize("Delete")]
        public async Task<IActionResult> DeleteRolePerrmission(int roleId, int permissionId)
        {
            (bool Success, string ErrorMessage) result = await _rolePermissionService.DeleteRolePermission(roleId, permissionId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _rolePermissionService.GetAllRolePermission());
        }

        [HttpGet]
        //[CustomAuthorize("GetAll")]
        public async Task<IActionResult> GetAllRole()
        {
            return Ok(await _rolePermissionService.GetAllRolePermission());
        }

        [HttpGet("{RoleID}")]
        //[CustomAuthorize("GetSingle")]
        public async Task<IActionResult> GetPerrmissionInRole(int roleId)
        {
            IEnumerable<RolePermissionDTO> role = await _rolePermissionService.GetPerrmissionInRole(roleId);
            if (role == null) return BadRequest("Not found");

            return Ok(role);
        }
    }
}
