using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoyageController : ControllerBase
    {
        private readonly IVoyageService _service;
        public VoyageController(IVoyageService service)
        {
            _service = service;
        }
        [HttpGet("VoyageDirectSearch")]
        public IActionResult GetDirectVoyageSearch(string Originportid, string Destinationportid)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetDirectVoyageSearch(Originportid, Destinationportid);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
    }
}