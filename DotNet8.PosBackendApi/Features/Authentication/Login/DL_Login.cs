namespace DotNet8.PosBackendApi.Features.Authentication.Login;

public class DL_Login
{
    private readonly JwtTokenGenerate _tokenGenerator;
    private readonly AppDbContext _context;
    private readonly JwtModel _tokenModel;
    public DL_Login(JwtTokenGenerate tokenGenerator,
        AppDbContext context, IOptionsMonitor<JwtModel> tokenModel)
    {
        _tokenGenerator = tokenGenerator;
        _context = context;
        _tokenModel = tokenModel.CurrentValue;
    }

    public async Task<TokenResponseModel> Login(LoginRequestModel reqModel)
    {
        var responseModel = new TokenResponseModel();
        var staff = await _context.TblStaffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffName == reqModel.UserName);

        if (staff is null || staff.Password != reqModel.Password.ToHash(_tokenModel.Key))
        {
            responseModel = new TokenResponseModel
            {
                Message = new MessageResponseModel(false, "UserName and Password Incorrect.Please try again."),
            };
            goto result;
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
        responseModel = new TokenResponseModel
        {
            Message = new MessageResponseModel(true, "Login Successful"),
            AccessToken = accessToken,
        };
    result:
        return responseModel;
    }
}
