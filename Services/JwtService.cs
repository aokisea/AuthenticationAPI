using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        // Get JWT settings from configuration
        var jwtSettings = _config.GetSection("JwtSettings");

        // Validate required settings
        _issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer", "JWT Issuer is missing in configuration.");
        _audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience", "JWT Audience is missing in configuration.");
        var keyString = jwtSettings["Key"] ?? throw new ArgumentNullException("JwtSettings:Key", "JWT Key is missing in configuration.");

        // Convert key to bytes
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
    }

    public string GenerateToken(string username)
    {
        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString()) // Expiration in Unix time
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
