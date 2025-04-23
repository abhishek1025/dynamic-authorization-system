using System.Data;
using Dapper;
using dynamic_authorization.domain.Entities;
using dynamic_authorization.domain.Interfaces;
using dynamic_authorization.infrastructure.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace dynamic_authorization.infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperDbContext _dbContext;
    

    public UserRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        SqlConnection dbConnection = _dbContext.CreateConnection();
        
        await dbConnection.OpenAsync();
        
        string query = "SELECT * FROM fn_get_user_by_email(@Email)";
        
        var _user = await dbConnection.QueryFirstOrDefaultAsync<User>(query,  new { Email = email });
        
        await dbConnection.CloseAsync();
        
        return _user;
    }
    
    public async Task<int> AddAsync(User user)
    {
        SqlConnection dbConnection = _dbContext.CreateConnection();
        
        await dbConnection.OpenAsync();
        
        var parameters = new
        {
            name = user.Name,
            email = user.Email,
            password = user.Password,
            is_admin = user.Is_Admin
        };

        int rowsAffected = await dbConnection.ExecuteAsync(
            "sp_create_user",              
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        await dbConnection.CloseAsync();

        
        return rowsAffected;
    }

    public async Task<List<User>> GetAllAsync()
    {
        SqlConnection dbConnection = _dbContext.CreateConnection();
        
        await dbConnection.OpenAsync();
        
        string query = "SELECT * FROM fn_get_all_users()";
        
        var users = await dbConnection.QueryAsync<User>(query);;
        
        await dbConnection.CloseAsync();
        
        return users.ToList();
    }
}