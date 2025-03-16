using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    [Authorize] // 👈 Requires JWT Authentication
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get username from JWT

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { Message = "Invalid or expired token" });
        }

        return Ok(new { Username = username });
    }
}
