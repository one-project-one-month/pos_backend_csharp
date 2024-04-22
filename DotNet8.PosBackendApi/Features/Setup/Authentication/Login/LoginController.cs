using DotNet8.PosBackendApi.Models.Setup.Login;

namespace DotNet8.PosBackendApi.Features.Setup.Authentication.Login;

[Route("api/v1/auth/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly BL_Login _Login;

    public LoginController(BL_Login login)
    {
        _Login = login;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel reqModel)
    {
        var model = await _Login.Login(reqModel);
        return Ok(model);
    }
}

// public IActionResult Refresh(TokenResponseModel tokenResponse)
// {
//     // For simplicity, assume the refresh token is valid and stored securely
//     // var storedRefreshToken = _userService.GetRefreshToken(userId);
//
//     // Verify refresh token (validate against the stored token)
//     // if (storedRefreshToken != tokenResponse.RefreshToken)
//     //    return Unauthorized();
//
//     // For demonstration, let's just generate a new access token
//     //var newAccessToken =
//     //    JwtTokenGenerate.GenerateAccessTokenFromRefreshToken(tokenResponse.RefreshToken, _config["Jwt:Key"]);
//
//     //var response = new TokenResponseModel
//     //{
//     //    AccessToken = newAccessToken,
//     //    RefreshToken = tokenResponse.RefreshToken
//     //};
//
//     return Ok();
// }