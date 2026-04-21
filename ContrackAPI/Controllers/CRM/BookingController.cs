using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        APIResponse response = new APIResponse();
        public BookingController(IBookingService service)
        {
            _service = service;
        }
        [HttpPost("List")]
        public IActionResult GetBookingList([FromBody] BookingListFilter filter)
        {
            try
            {
                response = _service.GetBookingList(filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }
           
        }

        [HttpGet("GetByBookingUUID")]
        public IActionResult GetbookingByUUID(string bookinguuid)
        {
            try
            {
                response = _service.GetBookingByUUID(bookinguuid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }
        }

        [HttpGet("ContainerSelection")]
        public IActionResult GetContainerSelection(string bookinguuid)
        {
            try
            {
                response = _service.GetContainerSelection(bookinguuid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }
        }
        [HttpPost("SaveSelection")]
        public IActionResult SaveContainerSelection([FromBody] ContainerSelection bookingmodel)
        {
            try
            {
                response = _service.SaveContainerSelection(bookingmodel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse());
            }
        }
    }
}