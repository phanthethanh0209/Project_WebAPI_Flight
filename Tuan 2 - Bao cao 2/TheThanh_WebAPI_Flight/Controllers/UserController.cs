using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_RobotHeineken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _userService.GetAllUser());
        }

        [HttpGet("{UserName}")]
        public async Task<IActionResult> GetByNameUser(string username)
        {
            UserDTO user = await _userService.GetUserAsync(username);
            if (user == null) return BadRequest("Not found");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO createDto)
        {
            (bool Success, string ErrorMessage) result = await _userService.CreateUser(createDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _userService.GetAllUser());
        }

        [HttpPut("{UserID}")]
        public async Task<IActionResult> UpdateUser(int userid, UserDTO updateDto)
        {
            (bool Success, string ErrorMessage) result = await _userService.UpdateUser(userid, updateDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(await _userService.GetAllUser());
        }

        [HttpDelete("{UserID}")]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            (bool Success, string ErrorMessage) result = await _userService.DeleteUser(UserID);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(await _userService.GetAllUser());


        }
    }
}
