using DotNet8.PosBackendApi.Models;

namespace DotNet8.Pos.App.Models.Auth;

public class LoginResponseModel
{
    public string token { get; set; }
    public MessageResponseModel Message { get; set; }
}