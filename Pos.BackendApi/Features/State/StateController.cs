namespace Pos.BackendApi.Features.State;

[Route("api/v1/states")]
[ApiController]
public class StateController : BaseController
{
    private readonly StateService _state;
    private readonly ResponseModel _response;

    public StateController(
        IServiceProvider serviceProvider,
        StateService state,
        ResponseModel response) : base(serviceProvider)
    {
        _state = state;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetState()
    {
        try
        {
            var stateLst = await _state.GetState();
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
            var stateLst = await _state.GetState(pageNo, pageSize);
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
            var state = await _state.GetStateByCode(StateCode);
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
            var state = await _state.CreateState(requestModel);
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
            var state = await _state.UpdateState(id, requestModel);
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
            var state = await _state.DeleteState(id);
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