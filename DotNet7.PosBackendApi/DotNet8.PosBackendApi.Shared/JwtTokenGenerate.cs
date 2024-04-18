using DotNet8.PosBackendApi.Models.Setup.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DotNet8.PosBackendApi.Shared;

public class JwtTokenGenerate
{
    private readonly TokenModel _token;

    public JwtTokenGenerate(IOptionsMonitor<TokenModel> token)
    {
        _token = token.CurrentValue;
    }

    public string GenerateAccessToken(StaffModel staff)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _token.Jwt.Key;
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("id", staff.StaffId.ToString()),
                new Claim("StaffName", staff.StaffName.ToString()),
                new Claim("StaffCode", staff.StaffCode.ToString()),
                new Claim("TokenExpired", DateTime.UtcNow.AddMinutes(15).ToString("o")),
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
    {
        // Implement logic to generate a new access token from the refresh token
        // Verify the refresh token and extract necessary information (e.g., user ID)
        // Then generate a new access token

        // For demonstration purposes, return a new token with an extended expiry
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(15), // Extend expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}