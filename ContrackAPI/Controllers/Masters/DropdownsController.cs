using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownsController : ControllerBase
    {
        [HttpGet("MovesDropdown")]
        public IActionResult GetMovesDropdown([FromQuery] bool showempty = true)
        {
            APIResponse response = new APIResponse();

            try
            {
                var data = Dropdowns.GetMovesDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception)
            {
                response.Result = Common.ErrorMessage("Error while fetching moves dropdown");
            }

            return Ok(response);
        }
        [HttpGet("LocationDropdown")]
        public IActionResult GetLocationDropdown([FromQuery] bool showempty = true)
        {
            APIResponse response = new APIResponse();

            try
            {
                var data = Dropdowns.GetLocationDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception)
            {
                response.Result = Common.ErrorMessage("Error while fetching location dropdown");
            }

            return Ok(response);
        }
        [HttpGet("NewMovesDropdown")]
        public IActionResult GetNewMovesDropdown([FromQuery] bool showempty = true)
        {
            APIResponse response = new APIResponse();

            try
            {
                var data = Dropdowns.GetNewMovesDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception)
            {
                response.Result = Common.ErrorMessage("Error while fetching Moves dropdown");
            }

            return Ok(response);
        }
    }
}
