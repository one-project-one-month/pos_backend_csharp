namespace DotNet8.PosBackendApi.Models.Setup.Login;

public class LoginRequestModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponseModel
{
    public string token {  get; set; }
    public MessageResponseModel Message { get; set; }
}
