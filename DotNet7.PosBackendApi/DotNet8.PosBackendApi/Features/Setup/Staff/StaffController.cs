namespace DotNet8.PosBackendApi.Features.Setup.Staff;

[Route("api/v1/staffs")]
[ApiController]
public class StaffController : BaseController
{
    private readonly StaffService _staffService;
    private readonly DL_Staff _staff;
    private readonly ResponseModel _response;


    public StaffController(StaffService staffService, DL_Staff staff, ResponseModel response)
    {
        _staffService = staffService;
        _staff = staff;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetStaffs()
    {
        try
        {
            var model = await _staff.GetStaffs();
            var responseModel = _response.ReturnGet
            (model.MessageResponse.Message,
                model.DataList.Count,
                EnumPos.Staff,
                model.MessageResponse.IsSuccess,
                model.DataList);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaff(int id)
    {
        try
        {
            var item = await _staff.GetStaff(id);
            var responseModel = _response.ReturnById
            (item.MessageResponse.Message,
                EnumPos.Staff,
                item.MessageResponse.IsSuccess,
                item.Data);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(int id, [FromBody] StaffModel requestModel)
    {
        try
        {
            var model = await _staff.UpdateStaff(id, requestModel);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message, EnumPos.Staff, requestModel);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }

        ;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            var model = await _staff.DeleteStaff(id);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message, EnumPos.Staff);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}