namespace DotNet8.PosBackendApi.Features.Setup.Authentication.Login;

[Route("api/v1/auth/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly BL_Staff _staff;

    public LoginController(IConfiguration config, BL_Staff staff)
    {
        _config = config;
        _staff = staff;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(StaffModel reqModel)
    {
        // Authenticate user
        var item = await _staff.GetStaff(reqModel.StaffId);

        if (item is null)
            return Unauthorized();

        // Generate tokens
        // var accessToken = JwtTokenGenerate.GenerateAccessToken(item, _config["Jwt:Key"]);
        var refreshToken = JwtTokenGenerate.GenerateRefreshToken();

        // Save refresh token (for demo purposes, this might be stored securely in a database)
        // _userService.SaveRefreshToken(user.Id, refreshToken);

        var response = new TokenResponseModel
        {
            // AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh(TokenResponseModel tokenResponse)
    {
        // For simplicity, assume the refresh token is valid and stored securely
        // var storedRefreshToken = _userService.GetRefreshToken(userId);

        // Verify refresh token (validate against the stored token)
        // if (storedRefreshToken != tokenResponse.RefreshToken)
        //    return Unauthorized();

        // For demonstration, let's just generate a new access token
        var newAccessToken =
            JwtTokenGenerate.GenerateAccessTokenFromRefreshToken(tokenResponse.RefreshToken, _config["Jwt:Key"]);

        var response = new TokenResponseModel
        {
            AccessToken = newAccessToken,
            RefreshToken = tokenResponse.RefreshToken
        };

        return Ok(response);
    }
}