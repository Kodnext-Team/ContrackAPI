using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContrackAPI.Controllers.Container
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _service;
        APIResponse response = new APIResponse();
        public TrackingController(ITrackingService service)
        {
            _service = service;
        }
        [HttpPost("List")]
        public IActionResult GetTrackingList([FromBody] TrackingFilterPage filter)
        {
            try
            {
                response = _service.GetTrackingList(filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }
        }
        [HttpPost("SaveRecordMove")]
        public IActionResult SaveRecordMove([FromBody] TrackingDTO model)
        {
            try
            {
                response = _service.SaveTracking(model);
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
        [HttpGet("GetTrackingDetails")]
        public IActionResult GetTrackingDetails(string containeruuid, string bookinguuid)
        {
            try
            {
                response = _service.GetTrackingDetails(containeruuid, bookinguuid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }

        }
    }
}
