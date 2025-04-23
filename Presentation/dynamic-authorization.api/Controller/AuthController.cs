using dynamic_authorization.application.DTOs.User;
using dynamic_authorization.application.Services.Interfaces;
using dynamic_authorization.infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_authorization.api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
        {
            await _authServices.SignUp(dto);

            return RestResponse.Ok(message: "User created successfully.");
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
        {
            string token = await _authServices.SignIn(dto);

            return RestResponse.Ok(message: "User created successfully.", data: new
            {
                token
            });
        }
    }
}