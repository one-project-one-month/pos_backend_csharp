namespace DotNet8.PosBackendApi.Features.Setup.Authentication.Register;

[Route("api/v1/auth/[controller]")]
[ApiController]
public class RegisterController : BaseController
{
    private readonly DL_Staff _staff;
    private readonly ResponseModel _response;
    private readonly JwtTokenGenerate _token;

    public RegisterController(DL_Staff staff, ResponseModel response, JwtTokenGenerate token)
    {
        _staff = staff;
        _response = response;
        _token = token;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] StaffModel requestModel)
    {
        try
        {
            var model = await _staff.CreateStaff(requestModel);
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message, EnumPos.Staff, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = _token.GenerateRefreshToken(RefreshToken()),
                EnumPos = EnumPos.Staff,
                IsSuccess = model.IsSuccess,
                Message = model.Message,
                Item = requestModel
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}