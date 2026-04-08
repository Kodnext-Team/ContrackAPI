using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _service;
        public ContainerController(IContainerService service)
        {
            _service = service;
        }
        [HttpPost("List")]
        public IActionResult GetContainerList([FromBody] ContainerFilterPage filter)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetContainerList(filter);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [HttpGet("GetByContainerUUID")]
        public IActionResult GetContainerByUUID(string containeruuid)
        {
            APIResponse response = new APIResponse();
            try
            {
                var data = _service.GetContainerByUUID(containeruuid);
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