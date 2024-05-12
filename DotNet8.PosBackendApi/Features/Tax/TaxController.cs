namespace DotNet8.PosBackendApi.Features.Tax;

[Route("api/taxes")]
[ApiController]
public class TaxController : BaseController
{
    private readonly ResponseModel _response;
    private readonly BL_Tax _bL_Tax;

    public TaxController(ResponseModel response, BL_Tax bL_Tax, IServiceProvider serviceProvider): base(serviceProvider)
    {
        _response = response;
        _bL_Tax = bL_Tax;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaxList()
    {
        try
        {
            var lst = await _bL_Tax.GetTaxList();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = lst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = lst.MessageResponse.Message,
                Item = lst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}