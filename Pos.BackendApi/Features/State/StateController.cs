namespace DotNet8.PosBackendApi.Features.State;

[Route("api/v1/states")]
[ApiController]
public class StateController : BaseController
{
    private readonly BL_State _bL_State;
    private readonly ResponseModel _response;

    public StateController(
        IServiceProvider serviceProvider,
        BL_State bL_State,
        ResponseModel response) : base(serviceProvider)
    {
        _bL_State = bL_State;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetState()
    {
        try
        {
            var stateLst = await _bL_State.GetState();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = stateLst.DataLst.Count,
                IsSuccess = stateLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.State,
                Message = stateLst.MessageResponse.Message,
                Item = stateLst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }


    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetState(int pageNo, int pageSize)
    {
        try
        {
            var stateLst = await _bL_State.GetState(pageNo, pageSize);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.State,
                IsSuccess = stateLst.MessageResponse.IsSuccess,
                Message = stateLst.MessageResponse.Message,
                Item = stateLst.Data.State,
                PageSetting = stateLst.Data.PageSetting
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{StateCode}")]
    public async Task<IActionResult> GetStateByCode(string StateCode)
    {
        try
        {
            var state = await _bL_State.GetStateByCode(StateCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = state.MessageResponse.IsSuccess,
                EnumPos = EnumPos.State,
                Message = state.MessageResponse.Message,
                Item = state.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateState(StateModel requestModel)
    {
        try
        {
            var state = await _bL_State.CreateState(requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = state.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = state.Message,
                Item = requestModel
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateState(int id, StateModel requestModel)
    {
        try
        {
            var state = await _bL_State.UpdateState(id, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = state.IsSuccess,
                EnumPos = EnumPos.State,
                Message = state.Message,
                Item = requestModel
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteState(int id)
    {
        try
        {
            var state = await _bL_State.DeleteState(id);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = state.IsSuccess,
                EnumPos = EnumPos.State,
                Message = state.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}