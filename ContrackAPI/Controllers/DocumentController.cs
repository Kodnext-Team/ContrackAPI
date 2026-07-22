using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;

        APIResponse response = new APIResponse();

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("Upload")]
        public IActionResult Upload([FromForm] UploadDocumentRequest request)
        {
            try
            {
                response = _service.UploadDocument(request);
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetDocumentList")]
        public IActionResult GetDocumentList(string targetuuid, int documenttypeid)
        {
            try
            {
                response = _service.GetDocumentListUUID(
                    targetuuid,
                    documenttypeid);
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetDocumentByUUID")]
        public IActionResult GetDocumentByUUID(string documentuuid)
        {
            try
            {
                response = _service.GetDocumentByUUID(documentuuid);
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpDelete("DeleteDocument")]
        public IActionResult DeleteDocument(string documentuuid)
        {
            try
            {
                response = _service.DeleteDocument(documentuuid);
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
    }
}