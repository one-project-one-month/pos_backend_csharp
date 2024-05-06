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
                Count = townshipLst.DataLst.Count,
                IsSuccess = townshipLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Township,
                Message = townshipLst.MessageResponse.Message,
                Item = townshipLst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{TownshipCode}")]
    public async Task<IActionResult> GetTownshipByCode(string TownshipCode)
    {
        try
        {
            var township = await _bL_Township.GetTownshipByCode(TownshipCode);
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

        ;
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
        };
    }
}
