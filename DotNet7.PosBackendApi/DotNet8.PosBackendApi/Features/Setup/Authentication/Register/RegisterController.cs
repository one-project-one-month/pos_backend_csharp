namespace DotNet8.PosBackendApi.Features.Setup.Authentication.Register;

[Route("api/v1/auth/[controller]")]
[ApiController]
public class RegisterController : BaseController
{
    private readonly DL_Staff _staff;
    private readonly ResponseModel _response;

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] StaffModel requestModel)
    {
        try
        {
            var model = await _staff.CreateStaff(requestModel);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message, EnumPos.Staff, requestModel);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}