using System.Data;
using dynamic_authorization.infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly DapperDbContext _dbContext;

        public PingController(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {

            try
            {
                var connection = _dbContext.CreateConnection();
                await connection.OpenAsync();
                bool isOpen = connection.State == ConnectionState.Open;

                return Ok(new { dbConnected = isOpen, message = "Pong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { connected = false, error = ex.Message });
            }
        }
    }
}
