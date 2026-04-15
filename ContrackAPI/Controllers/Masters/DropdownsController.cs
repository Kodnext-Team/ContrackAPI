using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownsController : ControllerBase
    {
        APIResponse response = new APIResponse();

        [HttpGet("MovesDropdown")]
        public IActionResult GetMovesDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetMovesDropdown(showempty);
                response.Data = data;
            }
            catch (Exception)
            {
            }
            return Ok(response);
        }
        [HttpGet("LocationDropdown")]
        public IActionResult GetLocationDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetLocationDropdown(showempty);
                response.Data = data;
            }
            catch (Exception)
            {
            }
            return Ok(response);
        }
        [HttpGet("NewMovesDropdown")]
        public IActionResult GetNewMovesDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetNewMovesDropdown(showempty);
                response.Data = data;
            }
            catch (Exception)
            {
            }
            return Ok(response);
        }
        [HttpGet("VoyageSearch")]
        public IActionResult GetVoyageSearch([FromQuery] string SearchText, [FromQuery] bool createnew = false)
        {
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
                response.Data = list;
            }
            catch (Exception)
            {
            }
            return Ok(response);
        }
    }
}
