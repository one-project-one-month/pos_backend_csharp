namespace DotNet8.PosBackendApi.Features.Authentication.Login;

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

    private static void CheckLoginNullValue(LoginRequestModel reqModel)
    {
        if (string.IsNullOrEmpty(reqModel.UserName))
            throw new Exception("UserName is null.");

        if (string.IsNullOrEmpty(reqModel.Password))
            throw new Exception("Password is null.");
    }
}
