namespace Pos.BackendApi.Features.Township;

[Route("api/v1/townships")]
[ApiController]
public class TownshipController : BaseController
{
    private readonly TownshipService _township;
    private readonly ResponseModel _response;

    public TownshipController(
        IServiceProvider serviceProvider,
        TownshipService township,
        ResponseModel response) : base(serviceProvider)
    {
        _township = township;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetTownship()
    {
        try
        {
            var townshipLst = await _township.GetTownship();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = townshipLst.DataList.Count,
                IsSuccess = townshipLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = townshipLst.MessageResponse.Message,
                Item = townshipLst.DataList
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetTownship(int pageNo, int pageSize)
    {
        try
        {
            var townshipLst = await _township.GetTownship(pageNo, pageSize);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.Township,
                IsSuccess = townshipLst.MessageResponse.IsSuccess,
                Message = townshipLst.MessageResponse.Message,
                Item = townshipLst.Data.Township,
                PageSetting = townshipLst.Data.PageSetting
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{townshipCode}")]
    public async Task<IActionResult> GetTownshipByCode(string townshipCode)
    {
        try
        {
            var township = await _township.GetTownshipByCode(townshipCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = township.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = township.MessageResponse.Message,
                Item = township.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("GetTownshipByStateCode/{stateCode}")]
    public async Task<IActionResult> GetTownshipByStateCode(string stateCode)
    {
        try
        {
            var lstTownship = await _township.GetTownshipByStateCode(stateCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = lstTownship.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = lstTownship.MessageResponse.Message,
                Item = lstTownship.DataList
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTownship(TownshipModel requestModel)
    {
        try
        {
            var township = await _township.CreateTownship(requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = township.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = township.Message,
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
    public async Task<IActionResult> UpdateTownship(int id, TownshipModel requestModel)
    {
        try
        {
            var township = await _township.UpdateTownship(id, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = township.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = township.Message,
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
    public async Task<IActionResult> DeleteTownship(int id)
    {
        try
        {
            var township = await _township.DeleteTownship(id);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = township.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = township.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}