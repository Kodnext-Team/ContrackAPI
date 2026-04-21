using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _service;
        APIResponse response = new APIResponse();
        public ContainerController(IContainerService service)
        {
            _service = service;
        }
        [HttpPost("List")]
        public IActionResult GetContainerList([FromBody] ContainerFilterPage filter)
        {
            try
            {
                response = _service.GetContainerList(filter);
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
        [HttpGet("GetByContainerUUID")]
        public IActionResult GetContainerByUUID(string containeruuid)
        {
            try
            {
                response = _service.GetContainerByUUID(containeruuid);
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
    }
}