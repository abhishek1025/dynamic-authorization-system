using dynamic_authorization.domain.Entities;
using dynamic_authorization.domain.Enums;

namespace dynamic_authorization.domain.Interfaces;

public interface IUserPermissionRepository
{
    Task<int> GetPermissionCountAsync(string userId, string resource, string operation);
    
    Task<int> AddAsync(UserPermission userPermission);
    
    Task<int> DeleteAsync(UserPermission userPermission);

    Task<IEnumerable<object>> GetAllAsync();
}