using dynamic_authorization.application.DTOs.Permission;
using dynamic_authorization.application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionServices _permissionService;

        public PermissionsController(IPermissionServices permissionService)
        {
            _permissionService = permissionService;
        }

        [Authorize("PERMISSION:CREATE")]
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody]CreatePermissionDto createPermissionDto)
        {
            
            await _permissionService.AddPermissionAsync(createPermissionDto);
             
            return RestResponse.Ok(
                message: "New permission created successfully",
                statusCode: StatusCodes.Status201Created
            );
        }
        
        [Authorize("PERMISSION:DELETE")]
        [HttpDelete("delete/{user_id}/{resource}/{permission_id}")]
        public async Task<IActionResult> DeletePermission(Guid user_id, string resource, int permission_id)
        {
            CreatePermissionDto createPermissionDto = new()
            {
                userId = user_id,
                resource = resource,
                permissionId = permission_id
            };

            await _permissionService.DeletePermissionAsync(createPermissionDto);

            return RestResponse.Ok(
                message: "Permission deleted successfully",
                statusCode: StatusCodes.Status201Created
            );
        }

        [Authorize("PERMISSION:READ")]
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var data = await _permissionService.GetUsersWithPermissionsAsync();
            
            return RestResponse.Ok(data: data);
        }
    }
}
