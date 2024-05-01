namespace DotNet8.PosBackendApi.Features.Authentication.Login;

[Route("api/v1/auth/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly BL_Login _Login;

    public LoginController(BL_Login login) => _Login = login;

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestModel reqModel)
    {
        var model = await _Login.Login(reqModel);
        return Ok(model);
    }
}