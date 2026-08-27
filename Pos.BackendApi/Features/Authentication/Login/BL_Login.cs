namespace Pos.BackendApi.Features.Authentication.Login;

public class BL_Login
{
    private readonly DL_Login _dL_login;

    public BL_Login(DL_Login login) => _dL_login = login;

    public async Task<LoginResponseModel> Login(LoginRequestModel reqModel)
    {
        CheckLoginNullValue(reqModel);
        var model = await _dL_login.Login(reqModel);
        return model;
    }

    public Task<LoginResponseModel> Refresh(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));

        return _dL_login.Refresh(refreshToken);
    }

    public Task Revoke(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));

        return _dL_login.Revoke(refreshToken);
    }

    private static void CheckLoginNullValue(LoginRequestModel reqModel)
    {
        if (string.IsNullOrEmpty(reqModel.UserName))
            throw new ArgumentException("Username is required.", nameof(reqModel));

        if (string.IsNullOrEmpty(reqModel.Password))
            throw new ArgumentException("Password is required.", nameof(reqModel));
    }
}
