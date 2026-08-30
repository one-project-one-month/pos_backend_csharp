namespace Pos.BackendApi.Features.Authentication.Login;

[Route("api/v1/auth/login")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly LoginService _login;

    public LoginController(LoginService login) => _login = login;

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestModel reqModel)
    {
        var model = await _login.Login(reqModel);
        return model.Message.IsSuccess ? Ok(model) : Unauthorized(model);
    }

    [HttpPost("/api/v1/auth/refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestModel request)
    {
        var model = await _login.Refresh(request.RefreshToken);
        return model.Message.IsSuccess ? Ok(model) : Unauthorized(model);
    }

    [HttpPost("/api/v1/auth/revoke")]
    public async Task<IActionResult> Revoke(RevokeTokenRequestModel request)
    {
        await _login.Revoke(request.RefreshToken);
        return NoContent();
    }
}
