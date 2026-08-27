namespace DotNet8.PosBackendApi.Features.Authentication.Register;

[Route("api/v1/auth/register")]
[ApiController]
public class RegisterController : BaseController
{
    private readonly DL_Staff _staff;
    private readonly ResponseModel _response;

    public RegisterController(IServiceProvider serviceProvider, DL_Staff staff, ResponseModel response)
        : base(serviceProvider)
    {
        _staff = staff;
        _response = response;
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
                Token = RefreshToken(),
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