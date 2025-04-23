using System.Data;
using Dapper;
using dynamic_authorization.domain.Entities;
using dynamic_authorization.domain.Enums;
using dynamic_authorization.domain.Interfaces;
using dynamic_authorization.infrastructure.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace dynamic_authorization.infrastructure.Repositories;

public class UserPermssionRepository : IUserPermissionRepository
{
    private readonly DapperDbContext _dbContext;
    
    public UserPermssionRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> GetPermissionCountAsync(string userId, string resource, string operation)
    {
        using var dbConnection = _dbContext.CreateConnection();
        await dbConnection.OpenAsync();

        var parameters = new
        {
            user_id = userId,
            resource,
            operation,
        };
        

        var result = await dbConnection.ExecuteScalarAsync<int>( 
            "sp_check_user_permission",              
            parameters,
            commandType: CommandType.StoredProcedure
            );

        await dbConnection.CloseAsync();

        return result;
    }


    public async Task<int> AddAsync(UserPermission userPermission)
    {
        SqlConnection dbConnection = _dbContext.CreateConnection();
        
        await dbConnection.OpenAsync();
        
        var parameters = new
        {
            permission_id = userPermission.PermissionId,
            resource = userPermission.Resource,
            user_id = userPermission.UserId
        };

        int rowsAffected = await dbConnection.ExecuteAsync(
            "sp_add_user_permission",              
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        
        await dbConnection.CloseAsync();
        
        return rowsAffected;
    }

    public async Task<int> DeleteAsync(UserPermission userPermission)
    {
        SqlConnection dbConnection = _dbContext.CreateConnection();
        
        await dbConnection.OpenAsync();
        
        string query = "EXEC sp_delete_user_permission(@UserId, @Resource, @PermissionId)";
     
        var result = await dbConnection.ExecuteAsync(query, userPermission);
        
        await dbConnection.CloseAsync();
        
        return result;
    }

    public async Task<IEnumerable<object>> GetAllAsync()
    {
         var dbConnection = _dbContext.CreateConnection();
        await dbConnection.OpenAsync();

        string query = "EXEC sp_get_users_with_permissions";
        
        var data = await dbConnection.QueryAsync(query);
        
        await dbConnection.CloseAsync();

        var result = data
            .GroupBy(row => new { row.user_id, row.name, row.email, row.resource })
            .Select(group => new
            {
                id = group.Key.user_id,
                name = group.Key.name,
                email = group.Key.email,
                resource = group.Key.resource,
                operations = group.Select(op => new
                {
                    operation = op.operation,
                    id = op.permission_id
                }).ToList()
            }).ToList();


        return result;
    }

}