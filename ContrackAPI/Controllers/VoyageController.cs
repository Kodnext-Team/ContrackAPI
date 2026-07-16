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
        [AllowAnonymous]
        [HttpPost("List")]
        public IActionResult GetVoyageList([FromBody] VoyageFilter filter)
        {
            try
            {
                response = _service.GetVoyageList(filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }

        }
        [AllowAnonymous]
        [HttpGet("VoyageDirectSearch")]
        public IActionResult GetDirectVoyageSearch(string Originportid, string Destinationportid)
        {
            try
            {
                response = _service.GetDirectVoyageSearch(Originportid, Destinationportid);
                return Ok(response);
            }
            catch (Exception ex)
            {


                return Ok(new APIResponse());
            }
           
        }
    }
}