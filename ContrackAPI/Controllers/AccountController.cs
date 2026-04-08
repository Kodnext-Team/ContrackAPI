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
            APIResponse response = new APIResponse();

            try
            {
                var serviceResponse = _service.ValidateLogin(login);
                response.Result = serviceResponse.Result;
                response.Data = serviceResponse.Data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
       
    }
}