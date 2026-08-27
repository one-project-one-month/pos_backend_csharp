namespace DotNet8.PosBackendApi.Shared;

public class JwtTokenGenerate
{
    private readonly JwtModel _token;

    public JwtTokenGenerate(IOptionsMonitor<JwtModel> token)
    {
        _token = token.CurrentValue;
    }

    public string GenerateAccessToken(StaffModel staff)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _token.Key;
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", staff.StaffId.ToString()),
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

    public string GenerateRefreshTokenV1()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateRefreshToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var item = decodedToken.Claims.FirstOrDefault(x => x.Type == "TokenExpired");
        DateTime tokenExpired = Convert.ToDateTime(item?.Value);

        var staffId = decodedToken.Claims.FirstOrDefault(x => x.Type == "Id") ?? throw new Exception("Id is required.");
        var staffName = decodedToken.Claims.FirstOrDefault(x => x.Type == "StaffName") ??
                        throw new Exception("StaffName is required");
        var staffCode = decodedToken.Claims.FirstOrDefault(x => x.Type == "StaffCode") ??
                        throw new Exception("StaffCode is required.");
        var model = new StaffModel
        {
            StaffId = Convert.ToInt32(staffId.Value),
            StaffName = staffName.Value,
            StaffCode = staffCode.Value,
        };
        var refreshToken = DateTime.Now > tokenExpired ? GenerateAccessToken(model) : token;
        return refreshToken;
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