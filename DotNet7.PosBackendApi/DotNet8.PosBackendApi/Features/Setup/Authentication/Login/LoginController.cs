using Microsoft.Extensions.Options;

namespace DotNet8.PosBackendApi.Features.Setup.Authentication.Login;

[Route("api/v1/auth/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly JwtTokenGenerate _tokenGenerator;
    private readonly AppDbContext _context;
    private readonly JwtModel _tokenModel;

    public LoginController(JwtTokenGenerate tokenGenerator, AppDbContext context, IOptionsMonitor<JwtModel> tokenModel)
    {
        _tokenGenerator = tokenGenerator;
        _context = context;
        _tokenModel = tokenModel.CurrentValue;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(StaffModel reqModel)
    {
        try
        {
            var staff = await _context.TblStaffs.AsNoTracking()
                .FirstOrDefaultAsync(x => x.StaffName == reqModel.StaffName);

            if (staff is null || staff.Password != reqModel.Password.ToHash(_tokenModel.Key))
            {
                return Unauthorized();
            }

            var model = new StaffModel
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                StaffCode = staff.StaffCode,
                Position = staff.Position,
                MobileNo = staff.MobileNo,
            };

            var accessToken = _tokenGenerator.GenerateAccessToken(model);
            var response = new TokenResponseModel
            {
                AccessToken = accessToken,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating tokens: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
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