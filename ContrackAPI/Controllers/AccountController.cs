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
    }
}