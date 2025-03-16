using Microsoft.AspNetCore.Mvc;
using IgnacioBackendApp.Models;
using IgnacioBackendApp.Services;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly UserService _userService;
    private readonly PasswordService _passwordService;

    public AuthController(JwtService jwtService, UserService userService, PasswordService passwordService)
    {
        _jwtService = jwtService;
        _userService = userService;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_userService.UserExists(request.Username))
        {
            return BadRequest(new { Message = "Username already taken." });
        }

        string hashedPassword = _passwordService.HashPassword(request.Password);
        _userService.CreateUser(request.Username, hashedPassword, request.Email);

        var token = _jwtService.GenerateToken(request.Username);
        return Ok(new { Message = "User registered successfully.", Token = token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.GetUserByUsername(request.Username);
        if (user == null || user.PasswordHash != _passwordService.HashPassword(request.Password))
        {
            return Unauthorized(new { Message = "Invalid credentials." });
        }

        var token = _jwtService.GenerateToken(request.Username);
        return Ok(new { Token = token });
    }
}

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
