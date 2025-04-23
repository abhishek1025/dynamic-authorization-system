using dynamic_authorization.application.DTOs.User;
using dynamic_authorization.application.Services.Interfaces;
using dynamic_authorization.domain.Entities;
using dynamic_authorization.domain.Exceptions;
using dynamic_authorization.domain.Interfaces;
using static BCrypt.Net.BCrypt;

namespace dynamic_authorization.application.Services;

public class AuthServices : IAuthServices
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _iJwtTokenService;

    public AuthServices(IUserRepository userRepository, IJwtTokenService iJwtTokenService)
    {
        _userRepository = userRepository;
        _iJwtTokenService = iJwtTokenService;
    }

    public async Task<string> SignIn(SignInDto signInDto)
    {
        User? existingUser = await _userRepository.GetByEmailAsync(signInDto.Email);
        
        bool isPasswordMatched = existingUser != null && Verify(signInDto.password, existingUser.Password);

        if (!isPasswordMatched)
        {
            throw new BadRequestException("Password or Email is incorrect");
        }
        
        return _iJwtTokenService.GenerateToken(existingUser?.Id.ToString(), existingUser.Email);
    }

    public async Task SignUp(SignUpDto signUpDto)
    {
        
        User? existingUser = await _userRepository.GetByEmailAsync(signUpDto.Email);

        if (existingUser != null)
        {
            throw new BadRequestException("User with this email already exists.");
        }
        
        var salt = GenerateSalt(12);
        signUpDto.password = HashPassword(signUpDto.password, salt);
        
        User user = new User()
        {
            Name = signUpDto.Name,
            Email = signUpDto.Email,
            Is_Admin = signUpDto.IsAdmin,
            Password = signUpDto.password,
        };

        var result = await _userRepository.AddAsync(user);
        
        if (result == 0)
        {
            throw new BadRequestException("Unable to create User. Please try again later.");
        }
    }
}