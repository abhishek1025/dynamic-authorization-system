using dynamic_authorization.domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace dynamic_authorization.api.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public ResourceEnum Resource { get; }
    
    public PermissionOperationEnum Operation { get; }

    public PermissionRequirement(ResourceEnum resource, PermissionOperationEnum operation)
    {
        Resource = resource;
        Operation = operation;
    }
}