namespace DotNet8.PosBackendApi.Features.Township;

[Route("api/v1/townships")]
[ApiController]
public class TownshipController : BaseController
{
    private readonly BL_Township _bL_Township;
    private readonly ResponseModel _response;

    public TownshipController(
        IServiceProvider serviceProvider,
        BL_Township bL_Township,
        ResponseModel response) : base(serviceProvider)
    {
        _bL_Township = bL_Township;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetTownship()
    {
        try
        {
            var townshipLst = await _bL_Township.GetTownship();
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
            var townshipLst = await _bL_Township.GetTownship(pageNo, pageSize);
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
            var township = await _bL_Township.GetTownshipByCode(townshipCode);
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
            var lstTownship = await _bL_Township.GetTownshipByStateCode(stateCode);
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
            var township = await _bL_Township.CreateTownship(requestModel);
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
            var township = await _bL_Township.UpdateTownship(id, requestModel);
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
            var township = await _bL_Township.DeleteTownship(id);
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