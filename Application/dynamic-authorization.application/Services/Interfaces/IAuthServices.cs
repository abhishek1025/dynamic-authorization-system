using dynamic_authorization.application.DTOs.User;
using dynamic_authorization.domain.Entities;

namespace dynamic_authorization.application.Services.Interfaces;

public interface IAuthServices
{
    Task<string> SignIn(SignInDto signInDto);
    Task SignUp(SignUpDto signUpDto);
}