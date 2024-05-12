using DotNet8.PosBackendApi.Models.Setup.Tax;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.Features.Tax;

[Route("api/taxes")]
[ApiController]
public class TaxController : BaseController
{
    private readonly ResponseModel _response;
    private readonly BL_Tax _bL_Tax;

    public TaxController(ResponseModel response, BL_Tax bL_Tax, IServiceProvider serviceProvider) : base(serviceProvider)
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
                EnumPos = EnumPos.Tax,
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaxById(int id)
    {
        try
        {
            var item = await _bL_Tax.GetTaxById(id);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = item.MessageResponse.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = item.MessageResponse.Message,
                    Item = item
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTax([FromBody] TaxModel requestModel)
    {
        try
        {
            var tax = await _bL_Tax.CreateTax(requestModel);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = tax.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = tax.Message,
                    Item = requestModel
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}