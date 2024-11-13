using Microsoft.AspNetCore.Mvc;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Services;

namespace TheThanh_WebAPI_Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            TokenDTO token = await _authService.AuthenticateAsync(loginDTO.Email, loginDTO.Password);

            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            return Ok(new
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenDTO model)
        {
            (bool success, string message, TokenDTO token) = await _authService.RenewToken(model);
            if (!success)
            {
                return BadRequest(new { Success = false, Message = message });
            }

            return Ok(new { Success = true, Data = token });
        }
    }
}
