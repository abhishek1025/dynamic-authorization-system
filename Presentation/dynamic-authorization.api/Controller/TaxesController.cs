using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        [Authorize("TAX:CREATE")]
        [HttpPost]
        public IActionResult CreateTaxes()
        {
            return RestResponse.Ok();
        }
        
        [Authorize("TAX:READ")]
        [HttpGet]
        public IActionResult GetTaxes()
        {
            return RestResponse.Ok();
        }
        
        [Authorize("TAX:UPDATE")]
        [HttpPut]
        public IActionResult UpdateTaxes()
        {
            return RestResponse.Ok();
        }
        
        [Authorize("TAX:DELETE")]
        [HttpDelete]
        public IActionResult DeleteTaxes()
        {
            return RestResponse.Ok();
        }
    }
}
