using DotNet8.PosBackendApi.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotNet8.PosBackendApi.Features;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private readonly JwtTokenGenerate _token;

    public BaseController(JwtTokenGenerate token)
    {
        _token = token;
    }

    protected IActionResult InternalServerError(Exception ex)
    {
        return StatusCode(500, new
        {
            Message = ex.ToString()
        });
    }

    protected IActionResult Content(object obj)
    {
        return Content(JsonConvert.SerializeObject(obj), "application/json");
    }

    protected string RefreshToken()
    {
        var token = Request.Headers
                .FirstOrDefault(x => x.Key == "Authorization")
                .Value
                .ToString()
                .Substring("Bearer ".Length) ?? throw new Exception("Invalid Token.");
        var refreshToken = _token.GenerateRefreshToken(token);
        return refreshToken;
    }
}

//public static class JwtTokenGenerate
//{
//    public static string GenerateAccessToken(StaffModel staff, string secret)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(secret);

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[] { new Claim("id", staff.StaffId.ToString()) }),
//            Expires = DateTime.UtcNow.AddHours(3),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
//                SecurityAlgorithms.HmacSha256Signature)
//        };

//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }

//    public static string GenerateRefreshToken()
//    {
//        var randomNumber = new byte[32];
//        using var rng = RandomNumberGenerator.Create();
//        rng.GetBytes(randomNumber);
//        return Convert.ToBase64String(randomNumber);
//    }

//    public static string GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
//    {
//        // Implement logic to generate a new access token from the refresh token
//        // Verify the refresh token and extract necessary information (e.g., user ID)
//        // Then generate a new access token

//        // For demonstration purposes, return a new token with an extended expiry
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(secret);

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Expires = DateTime.UtcNow.AddMinutes(15), // Extend expiration time
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
//                SecurityAlgorithms.HmacSha256Signature)
//        };

//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }
//}