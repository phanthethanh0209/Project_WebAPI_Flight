using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleUserController : ControllerBase
    {

        private readonly IRoleUserService _roleUserService;

        public RoleUserController(IRoleUserService roleUserService)
        {
            _roleUserService = roleUserService;
        }

        [HttpPost]
        //[CustomAuthorize("Create")]
        public async Task<IActionResult> AddUserToRole(RoleUserDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _roleUserService.AddUserToRole(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _roleUserService.GetAllRoleUser());
        }

        [HttpDelete("{RoleID}/{UserID}")]
        //[CustomAuthorize("Delete")]
        public async Task<IActionResult> DeleteRole(int roleId, int userId)
        {
            (bool Success, string ErrorMessage) result = await _roleUserService.DeleteRoleUser(roleId, userId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _roleUserService.GetAllRoleUser());
        }

        [HttpGet]
        //[CustomAuthorize("GetAll")]
        public async Task<IActionResult> GetAllRole()
        {
            return Ok(await _roleUserService.GetAllRoleUser());
        }

        [HttpGet("{RoleID}")]
        //[CustomAuthorize("GetSingle")]
        public async Task<IActionResult> GetUserInRole(int roleId)
        {
            IEnumerable<RoleUserDTO> role = await _roleUserService.GetUserInRole(roleId);
            if (role == null) return BadRequest("Not found");

            return Ok(role);
        }
    }
}
