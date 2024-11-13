using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;
using TheThanh_WebAPI_Flight.Validation;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IAuthService
    {
        Task<TokenDTO> AuthenticateAsync(string email, string password);
        Task<TokenDTO> GenerateToken(User user);
        Task<(bool Success, string ErrorMessage, TokenDTO Token)> RenewToken(TokenDTO token);
        public string GenerateRefreshToken();
    }

    public class AuthService : IAuthService
    {
        private readonly MyDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthValidator _authValidator;
        private readonly IRepositoryWrapper _repository;


        public AuthService(MyDBContext context, IConfiguration configuration, AuthValidator authValidator, IRepositoryWrapper repository)
        {
            _context = context;
            _configuration = configuration;
            _authValidator = authValidator;
            _repository = repository;
        }

        public async Task<TokenDTO> AuthenticateAsync(string email, string password)
        {
            // Tìm người dùng với email và mật khẩu 
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            // Sinh token nếu người dùng hợp lệ
            TokenDTO tokenDTO = await GenerateToken(user);

            return tokenDTO;
        }

        public string GenerateRefreshToken()
        {
            byte[] random = new Byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public async Task<TokenDTO> GenerateToken(User user)
        {
            // Lấy cài đặt JWT từ cấu hình
            IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityTokenHandler tokenHandler = new(); // tạo và xác thực JWT.
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.UtcNow.AddSeconds(20),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha512Signature)
                //SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                //                            SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); // Tạo token
            string accessToken = tokenHandler.WriteToken(token); // Chuyển đổi token thành chuỗi
            string refreshToken = GenerateRefreshToken();

            //Lưu database
            RefreshToken refreshTokenEntity = new()
            {
                JwtId = token.Id,
                UserId = user.UserID,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IsssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
            };

            await _repository.RefreshToken.CreateAsync(refreshTokenEntity);
            await _repository.SaveChangeAsync();

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<(bool Success, string ErrorMessage, TokenDTO Token)> RenewToken(TokenDTO model)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            // Định nghĩa các thông số xác thực token
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false, // Không kiểm tra issuer và audience của token.
                ValidateAudience = false,
                ValidateIssuerSigningKey = true, // Kiểm tra khóa ký của token.
                IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa ký từ cài đặt JWT.
                ClockSkew = TimeSpan.Zero, // Bỏ qua khoảng thời gian lệch
                ValidateLifetime = false //  Không kiểm tra thời hạn của token (vì token đã hết hạn).
            };

            AuthValidator tokenValidator = new(_context, tokenHandler, tokenValidationParameters);

            FluentValidation.Results.ValidationResult validationResult = tokenValidator.Validate(model);
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return (false, errors, null);
            }

            RefreshToken? storedToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
            // Cập nhật trạng thái của Refresh Token
            storedToken.IsRevoked = true; // đánh dấu token đã đc sử dụng
            storedToken.IsUsed = true; // đánh dấu token đã bị thu hồi
            _context.Update(storedToken);
            await _context.SaveChangesAsync();

            // Lấy người dùng từ cơ sở dữ liệu dựa trên UserId của storedToken để tạo token mới cho người dùng.
            User? user = await _context.Users.SingleOrDefaultAsync(t => t.UserID == storedToken.UserId);
            TokenDTO token = await GenerateToken(user);

            return (true, null, token);
        }
    }
}
