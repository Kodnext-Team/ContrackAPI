using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownsController : ControllerBase
    {
        private readonly IVoyageService _service;
        public DropdownsController(IVoyageService service)
        {
            _service = service;
        }
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
        [HttpGet("VoyageSearch")]
        public IActionResult GetVoyageSearch([FromQuery] string SearchText, [FromQuery] bool createnew = false)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = Dropdowns.GetVoyageSearch(SearchText, createnew);
                var list = data.Select(g => new
                {
                    NumericValue = g.VoyageId?.NumericValue,
                    EncryptedValue = g.VoyageId?.EncryptedValue,
                    Text = g.VoyageNumber,
                    Displaytext = g.ActualVoyageNumber,
                    VesselId = g.VesseDetailId?.EncryptedValue,
                    VesselName = g.Vesselname,
                    Comments = g.Description
                }).ToList();
                response.Result = Common.SuccessMessage("Success");
                response.Data = list;
            }
            catch (Exception)
            {
                response.Result = Common.ErrorMessage("Error while fetching voyage search");
            }
            return Ok(response);
        }
    }
}
