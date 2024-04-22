namespace DotNet8.PosBackendApi.Models.Setup.Token;

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
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}