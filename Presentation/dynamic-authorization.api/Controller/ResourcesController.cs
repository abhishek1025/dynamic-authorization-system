using dynamic_authorization.domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllModules()
        {
            var resources = Enum.GetNames(typeof(ResourceEnum)).Cast<string>();
            return RestResponse.Ok(data: resources);
        }
    }
}
