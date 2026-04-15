using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Container
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _service;
        private readonly IBookingService _bookingService;
        APIResponse response = new APIResponse();
        public TrackingController(ITrackingService service, IBookingService bookingService)
        {
            _service = service;
            _bookingService = bookingService;
        }
        [HttpPost("List")]
        public IActionResult GetTrackingList([FromBody] TrackingFilterPage filter)
        {
            try
            {
                var data = _service.GetTrackingList(filter);
                response.Data = data;
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
        [HttpPost("SaveRecordMove")]
        public IActionResult SaveRecordMove([FromBody] TrackingDTO model)
        {
            try
            {
                var result = _service.SaveTracking(model);
                response.Result = result;
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
                var booking = _bookingService.GetbookingByUUID(bookinguuid);
                var trackingDetails = _service.GetTrackingDetails(containeruuid, bookinguuid, booking);
                trackingDetails.booking = booking;
                response.Data = trackingDetails;
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
    }
}
