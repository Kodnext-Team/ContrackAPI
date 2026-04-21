using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoyageController : ControllerBase
    {
        private readonly IVoyageService _service;
        APIResponse response = new APIResponse();
        public VoyageController(IVoyageService service)
        {
            _service = service;
        }
        [HttpGet("VoyageDirectSearch")]
        public IActionResult GetDirectVoyageSearch(string Originportid, string Destinationportid)
        {
            try
            {
                var data = _service.GetDirectVoyageSearch(Originportid, Destinationportid);
                if (data != null)
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = data;
                }
                else
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
    }
}