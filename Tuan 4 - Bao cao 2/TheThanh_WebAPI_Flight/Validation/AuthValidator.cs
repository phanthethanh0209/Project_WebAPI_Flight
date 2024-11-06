using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Validation
{
    public class AuthValidator : AbstractValidator<TokenDTO>
    {
        private readonly MyDBContext _db;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthValidator(MyDBContext context, JwtSecurityTokenHandler tokenHandler, TokenValidationParameters tokenValidationParameters)
        {
            _db = context;
            _tokenHandler = tokenHandler;
            _tokenValidationParameters = tokenValidationParameters;

            // C1
            //RuleFor(token => token.AccessToken)
            //    .Must(BeValidAccessToken).WithMessage("Invalid access token")
            //    .Must(BeValidAlgAccessToken).WithMessage("Invalid token algorithm")
            //    .Must(BeValidExpiredAccessToken).WithMessage("Access token has not yet expired");

            //RuleFor(token => token.RefreshToken)
            //    .Must(BeValidExistRefreshToken).WithMessage("Refresh token does not exist")
            //    .Must(BeRevokeRefreshToken).WithMessage("Refresh token has been used or revoked");

            //RuleFor(token => token.RefreshToken)
            //    .Must(BeMatchRefreshToken).WithMessage("Token doesn't match");


            // trả về false mới báo lỗi
            RuleFor(x => x.AccessToken)
               .Custom((accessToken, context) =>
               {
                   if (!BeValidAccessToken(accessToken))
                   {
                       context.AddFailure("Access Token wrong format.");
                   }
                   else if (!BeValidAlgAccessToken(accessToken))
                   {
                       context.AddFailure("Invalid token algorithm.");
                   }
                   else if (!BeValidExpiredAccessToken(accessToken))
                   {
                       context.AddFailure("Access Token has not yet expired.");
                   }
               });

            RuleFor(x => x.RefreshToken)
                .Custom((refreshToken, context) =>
                {
                    if (!BeValidExistRefreshToken(refreshToken))
                    {
                        context.AddFailure("Refresh Token does not exist.");
                    }
                    else if (!BeRevokeRefreshToken(refreshToken))
                    {
                        context.AddFailure("Refresh Token has been revoked.");
                    }
                });
        }

        // check định dạng của Access Token
        private bool BeValidAccessToken(string accessToken)
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal tokenInVerification = _tokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out SecurityToken? validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        // check thuật toán mã hóa của token khớp với thuật toán server
        private bool BeValidAlgAccessToken(string accessToken)
        {
            System.Security.Claims.ClaimsPrincipal tokenInVerification = _tokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out SecurityToken? validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                return result;
            }

            return false;
        }

        // check thời hạn của Access Token
        private bool BeValidExpiredAccessToken(string accessToken)
        {
            System.Security.Claims.ClaimsPrincipal tokenInVerification = _tokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out SecurityToken? validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                long utcExpireDate = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Exp)?.Value ?? "0");

                DateTime expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                return expireDate <= DateTime.UtcNow;
            }

            return true;
        }

        // Check refresh token có tồn tại ko
        private bool BeValidExistRefreshToken(string refreshToken)
        {
            RefreshToken? storedToken = _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken).Result;

            return storedToken != null;
        }

        // Check refresh Token có bị thu hồi ko
        private bool BeRevokeRefreshToken(string refreshToken)
        {
            RefreshToken? storedToken = _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken).Result;

            return storedToken != null && !storedToken.IsRevoked;
        }


        private DateTime ConvertUnixTimeToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }
    }
}
