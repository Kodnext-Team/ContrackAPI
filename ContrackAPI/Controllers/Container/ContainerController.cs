using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet("GetByContainerEquipmentno")]
        public IActionResult GetContainerByEquipmentno(string equipmentno)
        {
            try
            {
                response = _service.GetContainerByEquipmentno(equipmentno);
            }
            catch (Exception ex)
            {
            }
            return Ok(response);
        }
    }
}