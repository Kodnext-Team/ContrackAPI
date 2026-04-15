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
                var data = _service.GetBookingList(filter);
               // response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
               // response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }

        [HttpGet("GetByBookingUUID")]
        public IActionResult GetbookingByUUID(string bookinguuid)
        {
            try
            {
                var data = _service.GetbookingByUUID(bookinguuid);
                response.Data = data;
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }

        [HttpGet("ContainerSelection")]
        public IActionResult GetContainerSelection(string bookinguuid)
        {
            try
            {
                var data = _service.GetContainerSelection(bookinguuid);
                response.Data = data;
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
    }
}