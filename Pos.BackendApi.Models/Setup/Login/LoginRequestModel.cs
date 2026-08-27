using System.ComponentModel.DataAnnotations;

namespace Pos.BackendApi.Models.Setup.Login;

public class LoginRequestModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAtUtc { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAtUtc { get; set; }
    public AuthenticatedStaffModel? Staff { get; set; }
    public MessageResponseModel Message { get; set; } = new();
}

public sealed class AuthenticatedStaffModel
{
    public int StaffId { get; set; }
    public string StaffCode { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}

public sealed class RefreshTokenRequestModel
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class RevokeTokenRequestModel
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
