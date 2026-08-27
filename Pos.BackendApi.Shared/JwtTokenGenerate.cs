namespace Pos.BackendApi.Shared;

public class JwtTokenGenerate
{
    private readonly JwtModel _token;

    public JwtTokenGenerate(IOptionsMonitor<JwtModel> token)
    {
        _token = token.CurrentValue;
    }

    public string GenerateAccessToken(StaffModel staff)
        => GenerateAccessTokenWithExpiry(staff).Token;

    public AccessTokenResult GenerateAccessTokenWithExpiry(StaffModel staff)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _token.Key;
        var key = Encoding.ASCII.GetBytes(secret);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_token.AccessTokenMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, staff.StaffId.ToString()),
                new Claim(ClaimTypes.Name, staff.StaffName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim("StaffId", staff.StaffId.ToString()),
                new Claim("StaffName", staff.StaffName.ToString()),
                new Claim("StaffCode", staff.StaffCode.ToString()),
                new Claim("Position", staff.Position ?? string.Empty),
            }),
            Issuer = _token.Issuer,
            Audience = _token.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = expiresAtUtc,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessTokenResult(tokenHandler.WriteToken(token), expiresAtUtc);
    }

    public static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public static byte[] HashRefreshToken(string refreshToken)
        => SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

    [Obsolete("Use the rotating refresh-token endpoints instead.")]
    public string GenerateRefreshToken(string token)
    {
        return token;
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

public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc);
