namespace Pos.BackendApi.Features.Authentication.Login;

[Route("api/v1/auth/login")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly BL_Login _Login;

    public LoginController(BL_Login login) => _Login = login;

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestModel reqModel)
    {
        var model = await _Login.Login(reqModel);
        return model.Message.IsSuccess ? Ok(model) : Unauthorized(model);
    }

    [HttpPost("/api/v1/auth/refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestModel request)
    {
        var model = await _Login.Refresh(request.RefreshToken);
        return model.Message.IsSuccess ? Ok(model) : Unauthorized(model);
    }

    [HttpPost("/api/v1/auth/revoke")]
    public async Task<IActionResult> Revoke(RevokeTokenRequestModel request)
    {
        await _Login.Revoke(request.RefreshToken);
        return NoContent();
    }
}
