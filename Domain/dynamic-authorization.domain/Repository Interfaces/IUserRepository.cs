using dynamic_authorization.domain.Entities;

namespace dynamic_authorization.domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(String email);
    Task<int> AddAsync(User user);
    
    Task<List<User>> GetAllAsync();
}
