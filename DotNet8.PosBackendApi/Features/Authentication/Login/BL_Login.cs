namespace DotNet8.PosBackendApi.Features.Authentication.Login;

public class BL_Login
{
    private readonly DL_Login _DL_login;

    public BL_Login(DL_Login login) => _DL_login = login;

    public async Task<TokenResponseModel> Login(LoginRequestModel reqModel)
    {
        CheckLoginNullValue(reqModel);
        //if (reqModel is null ) throw new Exception("UserName and password cannot be null");
        var model = await _DL_login.Login(reqModel);
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
