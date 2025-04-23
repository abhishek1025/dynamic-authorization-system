using dynamic_authorization.application.DTOs.Permission;
using dynamic_authorization.application.Services.Interfaces;
using dynamic_authorization.domain.Entities;
using dynamic_authorization.domain.Exceptions;
using dynamic_authorization.domain.Interfaces;

namespace dynamic_authorization.application.Services;

public class PermissionServices : IPermissionServices
{
    private readonly IUserPermissionRepository _userPermissionRepository;
    
    public PermissionServices(IUserPermissionRepository userPermissionRepository)
    {
        _userPermissionRepository = userPermissionRepository;
    }
    
    public Task<int> CheckUserPermissionAsync(string userId, string resource, string operation)
    {
       return _userPermissionRepository.GetPermissionCountAsync(userId, resource, operation);
    }

    public async Task<int> AddPermissionAsync(CreatePermissionDto createPermissionDto)
    {

        UserPermission userPermission = new()
        {
            PermissionId = createPermissionDto.permissionId,
            UserId = createPermissionDto.userId,
            Resource = createPermissionDto.resource
        };

        var result = await _userPermissionRepository.AddAsync(userPermission);

        if (result == 0)
        {
            throw new BadRequestException("Unable to create Permission. Please try again later.");
        }
        
        return result;
    }

    public async Task<int> DeletePermissionAsync(CreatePermissionDto createPermissionDto)
    {
        UserPermission userPermission = new()
        {
            PermissionId = createPermissionDto.permissionId,
            UserId = createPermissionDto.userId,
            Resource = createPermissionDto.resource
        };
        
        var result = await _userPermissionRepository.DeleteAsync(userPermission);
        
        if (result == 0)
        {
            throw new BadRequestException("Unable to delete Permission. Please try again later.");
        }
        
        return result;
    }

    public async Task<IEnumerable<object>> GetUsersWithPermissionsAsync()
    {
        return await _userPermissionRepository.GetAllAsync();
    }
}