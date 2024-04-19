using System.Collections.Generic;

namespace DotNet8.PosBackendApi.Features.Setup.Staff;

[Route("api/v1/staffs")]
[ApiController]
public class StaffController : BaseController
{
    private readonly StaffService _staffService;
    private readonly DL_Staff _staff;
    private readonly ResponseModel _response;
    private readonly JwtTokenGenerate _token;

    public StaffController(StaffService staffService, DL_Staff staff, ResponseModel response, JwtTokenGenerate token)
    {
        _staffService = staffService;
        _staff = staff;
        _response = response;
        _token = token;
    }

    [HttpGet]
    public async Task<IActionResult> GetStaffs()
    {
        try
        {
            var lst = await _staff.GetStaffs();
            //var responseModel = _response.ReturnGet
            //(model.MessageResponse.Message,
            //    model.DataList.Count,
            //    EnumPos.Staff,
            //    model.MessageResponse.IsSuccess,
            //    model.DataList);
            var responseModel = _response.Return
           (new ReturnModel
           {
               Token = _token.GenerateRefreshToken(RefreshToken()),
               Count = lst.DataList.Count,
               EnumPos = EnumPos.Staff,
               IsSuccess = lst.MessageResponse.IsSuccess,
               Message = lst.MessageResponse.Message,
               Item = lst.DataList
           });
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
            //var responseModel = _response.ReturnById
            //(item.MessageResponse.Message,
            //    EnumPos.Staff,
            //    item.MessageResponse.IsSuccess,
            //    item.Data);
            var responseModel = _response.Return
           (new ReturnModel
           {
               Token = _token.GenerateRefreshToken(RefreshToken()),
               EnumPos = EnumPos.Staff,
               IsSuccess = item.MessageResponse.IsSuccess,
               Message = item.MessageResponse.Message,
               Item = item.Data
           });
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
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message, EnumPos.Staff, requestModel);
            var responseModel = _response.Return
           (new ReturnModel
           {
               Token = _token.GenerateAccessToken(requestModel),
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(int id, [FromBody] StaffModel requestModel)
    {
        try
        {
            var model = await _staff.UpdateStaff(id, requestModel);
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

        ;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            var model = await _staff.DeleteStaff(id);
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message, EnumPos.Staff);
            var responseModel = _response.Return
          (new ReturnModel
          {
              Token = _token.GenerateRefreshToken(RefreshToken()),
              EnumPos = EnumPos.Staff,
              IsSuccess = model.IsSuccess,
              Message = model.Message,
          });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}