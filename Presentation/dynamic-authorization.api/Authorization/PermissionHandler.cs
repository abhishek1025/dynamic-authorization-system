using dynamic_authorization.application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace dynamic_authorization.api.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionServices _permissionService;
    private readonly IJwtTokenService _jwtTokenService;
    
    public PermissionHandler(IPermissionServices permissionService, IJwtTokenService jwtTokenService)
    {
        _permissionService = permissionService;
        _jwtTokenService = jwtTokenService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var userId = _jwtTokenService.GetUserIdFromToken();
        
        if (string.IsNullOrEmpty(userId))
        {
            return; 
        }

        bool hasPermission = await _permissionService.CheckUserPermissionAsync(userId, requirement.Resource.ToString(), requirement.Operation.ToString()) > 0;

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        
        
    }
    
}