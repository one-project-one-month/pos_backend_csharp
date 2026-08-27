namespace Pos.BackendApi.Models.Setup.Token;

public class TokenResponseModel
{
    public string AccessToken { get; set; }
    public MessageResponseModel Message { get; set; }
}

public class TokenModel
{
    public JwtModel Jwt {  get; set; }
}

public class JwtModel
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
}
