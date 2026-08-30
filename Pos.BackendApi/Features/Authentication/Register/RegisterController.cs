namespace Pos.BackendApi.Features.Authentication.Register;

[Route("api/v1/auth/register")]
[ApiController]
public class RegisterController : BaseController
{
    private readonly RegisterService _register;
    private readonly ResponseModel _response;

    public RegisterController(IServiceProvider serviceProvider, RegisterService register, ResponseModel response)
        : base(serviceProvider)
    {
        _register = register;
        _response = response;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] StaffModel requestModel)
    {
        try
        {
            var model = await _register.CreateStaff(requestModel);
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
