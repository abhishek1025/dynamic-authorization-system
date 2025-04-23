using dynamic_authorization.application.DTOs.Permission;
using dynamic_authorization.domain.Entities;

namespace dynamic_authorization.application.Services.Interfaces;

public interface IPermissionServices
{
    Task<int> CheckUserPermissionAsync(string userId, string resource, string operation);
    
    Task<int> AddPermissionAsync(CreatePermissionDto createPermissionDto);
    
    Task<int> DeletePermissionAsync(CreatePermissionDto createPermissionDto);
    
    Task<IEnumerable<object>> GetUsersWithPermissionsAsync();
}