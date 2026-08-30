using Pos.BackendApi.Models.Setup.Tax;
using System.Collections.Generic;

namespace Pos.BackendApi.Features.Tax;

[Route("api/v1/taxes")]
[ApiController]
public class TaxController : BaseController
{
    private readonly ResponseModel _response;
    private readonly TaxService _tax;

    public TaxController(ResponseModel response, TaxService tax, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _response = response;
        _tax = tax;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaxList()
    {
        try
        {
            var lst = await _tax.GetTaxList();
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
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetTaxList(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var lst = await _tax.GetTaxList(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = lst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Tax,
                Message = lst.MessageResponse.Message,
                Item = lst.DataLst,
                PageSetting = lst.PageSetting
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaxById(int id)
    {
        try
        {
            var item = await _tax.GetTaxById(id);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = item.MessageResponse.IsSuccess,
                    EnumPos = EnumPos.Tax,
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
    public async Task<IActionResult> CreateTax([FromBody] TaxModel requestModel)
    {
        try
        {
            var tax = await _tax.CreateTax(requestModel);
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
            return InternalServerError(ex);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTax([FromBody] TaxModel requestModel, int id)
    {
        try
        {
            var tax = await _tax.UpdateTax(id, requestModel);
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
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTax(int id)
    {
        try
        {
            var tax = await _tax.DeleteTax(id);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = tax.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = tax.Message,
                    Item = tax
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}