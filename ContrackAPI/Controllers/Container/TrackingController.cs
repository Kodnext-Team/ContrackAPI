using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Container
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _service;
        private readonly IBookingService _bookingService;
        public TrackingController(ITrackingService service, IBookingService bookingService)
        {
            _service = service;
            _bookingService = bookingService;
        }
        [HttpPost("List")]
        public IActionResult GetTrackingList([FromBody] TrackingFilterPage filter)
        {
            APIResponse response = new APIResponse();

            try
            {
                var data = _service.GetTrackingList(filter);
                response.Result = Common.SuccessMessage("Success");
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
            Result response = new Result();
            try
            {
                var result = _service.SaveTracking(model);
            }
            catch (Exception ex)
            {
                response = Common.ErrorMessage("Error while saving tracking");
            }
            return Ok(response);
        }
        [HttpGet("GetTrackingDetails")]
        public IActionResult GetTrackingDetails(string containeruuid, string bookinguuid)
        {
            APIResponse response = new APIResponse();

            try
            {
                var booking = _bookingService.GetbookingByUUID(bookinguuid);
                var trackingDetails = _service.GetTrackingDetails(containeruuid, bookinguuid, booking);
                trackingDetails.booking = booking;
                response.Result = Common.SuccessMessage("Success");
                response.Data = trackingDetails;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
    }
}
