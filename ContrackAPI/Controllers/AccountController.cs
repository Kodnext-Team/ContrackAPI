using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _service;
        public AccountController(ILoginService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUI login)
        {
            try
            {
                var response = _service.ValidateLogin(login);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new LoginResponse());
            }
        }

        [AllowAnonymous]
        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken([FromBody] TokenValidationRequest request)
        {
            try
            {
                var token = request?.Token;
                if (string.IsNullOrWhiteSpace(token))
                {
                    var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }

                var response = _service.ValidateToken(token);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new LoginResponse { Result = Common.ErrorMessage("An error occurred during token validation") });
            }
        }
    }
}