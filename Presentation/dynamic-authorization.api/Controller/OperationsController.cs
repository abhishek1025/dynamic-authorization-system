using System.Data;
using Dapper;
using dynamic_authorization.domain.Entities;
using dynamic_authorization.infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly DapperDbContext _dbContext;

        public OperationsController(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOperations()
        {
            SqlConnection dbConnection = _dbContext.CreateConnection();
            await dbConnection.OpenAsync();

            var data = await dbConnection.QueryAsync<Permission>(
                "sp_get_all_permissions",
                CommandType.StoredProcedure 
                );
            
            await dbConnection.CloseAsync();

            return RestResponse.Ok(data: data);
        }
    }
}
