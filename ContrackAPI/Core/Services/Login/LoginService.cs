using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContrackAPI
{
    public class LoginService : CustomException, ILoginService
    {
        private readonly ILoginRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly IHubRepository _HubRepository;

        public LoginService(ILoginRepository repo, IConfiguration configuration, IHubRepository hubRepository)
        {
            _repo = repo;
            _configuration = configuration;
            _HubRepository = hubRepository;
        }

        public LoginResponse ValidateLogin(LoginUI loginui)
        {
            var response = new LoginResponse();
            if (string.IsNullOrWhiteSpace(loginui.UserName) || string.IsNullOrWhiteSpace(loginui.Password))
            {
                response.Result = Common.ErrorMessage("Invalid Username/Password");
                return response;
            }
            var loginDto = new LoginDTO
            {
                UserName = loginui.UserName,
                Password = loginui.Password
            };
            Result validationResult = _repo.ValidateLogin(loginDto);
            response.Result = validationResult;
            if (validationResult.ResultId == 1)
            {
                response.Token = JwtTokenService.GenerateToken(loginDto.UserID.NumericValue, loginDto.HubID, _configuration);
                response.Data = new Login
                {
                    LoginInfo = loginDto,
                   // HubInfo = _HubRepository.GetHubByID(loginDto.HubID)
                };
            }
            return response;
        }
        public UserDTO GetUserById(int userId)
        {
            return _repo.GetUserByID(userId);
        }

        public LoginResponse ValidateToken(string token)
        {
            var response = new LoginResponse();
            if (string.IsNullOrWhiteSpace(token))
            {
                response.Result = Common.ErrorMessage("Token is required");
                return response;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration["Jwt:Key"] ?? "";
                var issuer = _configuration["Jwt:Issuer"] ?? "";
                var audience = _configuration["Jwt:Audience"] ?? "";
                var key = Encoding.UTF8.GetBytes(secretKey);

                bool shouldValidateLifetime = true;
                if (ExtendedTokenStore.TryGetExpiry(token, out DateTime customExpiry))
                {
                    if (DateTime.UtcNow < customExpiry)
                    {
                        shouldValidateLifetime = false;
                    }
                    else
                    {
                        response.Result = Common.ErrorMessage("Token expired");
                        return response;
                    }
                }

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = shouldValidateLifetime,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (Common.HttpContextAccessor?.HttpContext != null)
                {
                    Common.HttpContextAccessor.HttpContext.User = principal;
                }
                var jwtToken = (JwtSecurityToken)validatedToken;

                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId") ?? jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                var hubIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "HubId");

                if (userIdClaim == null || hubIdClaim == null)
                {
                    response.Result = Common.ErrorMessage("Invalid token claims");
                    return response;
                }

                int userId = int.Parse(userIdClaim.Value);
                int hubId = int.Parse(hubIdClaim.Value);

                var newExpiry = DateTime.UtcNow.AddDays(7);
                ExtendedTokenStore.ExtendToken(token, newExpiry);

                response.Result = Common.SuccessMessage("Token is valid. Extended for 7 days.");
                // Return the old token string with extended validity in the store
                response.Token = token;
                var user = _repo.GetUserByID(userId);
                if (user != null)
                {
                    var loginDto = new LoginDTO
                    {
                        UserID = user.UserID,
                        Type = user.Type,
                        EntityIDEncryptedList = user.EntityIDEncryptedList,
                        UserName = user.UserName,
                        Name = user.Name,
                        DateTimeCreated = user.DateTimeCreated,
                        Email = user.Email,
                        Phone = user.Phone,
                        HubID = user.HubID,
                        RoleID = user.RoleID,
                        RoleName = user.RoleName,
                        RoleIcon = user.RoleIcon,
                    };
                    response.Data = new Login
                    {
                        LoginInfo = loginDto
                    };
                }
            }
            catch (SecurityTokenExpiredException)
            {
                response.Result = Common.ErrorMessage("Token expired");
            }
            catch (Exception)
            {
                response.Result = Common.ErrorMessage("Invalid token");
            }

            return response;
        }
    }
}