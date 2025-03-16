namespace IgnacioBackendApp.Models;

public class LoginRequest
{
    public required string Username { get; set; }  //  Ensures it's required (C# 11+)
    public required string Password { get; set; }  //  Ensures it's required (C# 11+)
}
