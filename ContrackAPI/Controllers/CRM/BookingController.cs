using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        public IActionResult GetBookingList([FromBody] BookingListFilter filter)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetBookingList(filter);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }

        [HttpGet("GetByBookingUUID")]
        public IActionResult GetbookingByUUID(string bookinguuid, bool getsummary = false)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetbookingByUUID(bookinguuid, getsummary);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [HttpGet("ContainerSelection")]
        public IActionResult GetContainerSelection(string bookinguuid)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetContainerSelection(bookinguuid);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [HttpPost("SaveContainerSelection")]
        public IActionResult SaveContainerSelection([FromBody] ContainerSelection bookingmodel)
        {
            Result result = new Result();
            try
            {
                result = _service.SaveContainerSelection(
                    bookingmodel.Booking.bookingid.EncryptedValue,
                    bookingmodel.Selections
                );
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message);
            }

            return Ok(result);
        }

    }
}