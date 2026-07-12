using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContrackAPI.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownsController : ControllerBase
    {
        APIResponse response = new APIResponse();
        [AllowAnonymous]
        [HttpGet("MovesDropdown")]
        public IActionResult GetMovesDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetMovesDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("LocationDropdown")]
        public IActionResult GetLocationDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetLocationDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("NewMovesDropdown")]
        public IActionResult GetNewMovesDropdown([FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetNewMovesDropdown(showempty);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("VoyageSearch")]
        public IActionResult GetVoyageSearch([FromQuery] string SearchText, [FromQuery] bool createnew = false)
        {
            try
            {
                var data = Dropdowns.GetVoyageSearch(SearchText, createnew);
                response.Result = Common.SuccessMessage("Success");
                var list = data.Select(g => new
                {
                    NumericValue = g.VoyageId?.NumericValue,
                    EncryptedValue = g.VoyageId?.EncryptedValue,
                    Text = g.VoyageNumber,
                    Displaytext = g.ActualVoyageNumber,
                    VesselId = g.VesseDetailId?.EncryptedValue,
                    VesselName = g.Vesselname,
                    Comments = g.Description
                }).ToList();
                response.Data = list;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("PortDropdown")]
        public IActionResult GetPortDropdown([FromQuery] string countryid = "", [FromQuery] bool showempty = true)
        {
            try
            {
                var data = Dropdowns.GetPortDropdown(countryid, showempty);           
                response.Result = Common.SuccessMessage("Success");               
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("AgencyDropdown")]
        public IActionResult GetAgenciesUUIDDropdown([FromQuery] bool multiple = true)
        {
            try
            {
                var data = Dropdowns.GetAgenciesUUIDDropdown(multiple);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("ClientDropdown")]
        public IActionResult GetClientsByUserIDDropdown([FromQuery] bool multiple = false)
        {
            try
            {
                var data = Dropdowns.GetClientsByUserIDDropdown(multiple);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("CreatedByDropdown")]
        public IActionResult GetCreatedByDropdown()
        {
            try
            {
                var data = Dropdowns.GetLoginUsersByRole(Common.Encrypt(0), "",false,false);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("StatusDropdown")]
        public IActionResult GetStatusDropdown()
        {
            try
            {
                var data = Dropdowns.GetStatusDropdown(105, false);
                response.Result = Common.SuccessMessage("Success");
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("VesselDropdown")]
        public IActionResult GetVesselDropdown(string q = "", string AgencyDetailID = "", string multiple = "")
        {
            try
            {
                List<DropdownItem> results;

                if (string.IsNullOrEmpty(AgencyDetailID) || AgencyDetailID == "0")
                {
                    results = Dropdowns.GetVesselDropdownSearch(q, multiple);
                }
                else
                {
                    results = Dropdowns.GetVesselDropdown(AgencyDetailID, q, multiple);
                }
                var data = results.Select(x => new
                {
                    id = x.Value,
                    text = x.Text
                }).ToList();
                return Ok(new APIResponse
                {
                    Result = Common.SuccessMessage("Success"),
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    Result = Common.ErrorMessage(ex.Message)
                });
            }
        }
    }
}
