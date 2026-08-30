using System.Collections.Generic;

namespace Pos.BackendApi.Features.Product;

[Route("api/v1/products")]
[ApiController]
public class ProductController : BaseController
{
    private readonly ProductService _product;
    private readonly ResponseModel _response;
    private readonly JwtTokenGenerate _token;

    public ProductController(IServiceProvider serviceProvider, ProductService product, ResponseModel response,
        JwtTokenGenerate token) : base(serviceProvider)
    {
        _product = product;
        _response = response;
        _token = token;
    }

    [HttpGet]
    public async Task<IActionResult> GetProduct()
    {
        try
        {
            var productLst = await _product.GetProduct();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = productLst.DataLst.Count,
                IsSuccess = productLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = productLst.MessageResponse.Message,
                Item = productLst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{productCode}")]
    public async Task<IActionResult> GetProductByCode(string productCode)
    {
        try
        {
            var product = await _product.GetProductByCode(productCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = product.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = product.MessageResponse.Message,
                Item = product.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductModel requestModel)
    {
        try
        {
            var product = await _product.Create(requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = product.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = product.Message,
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
    public async Task<IActionResult> Update(int id, ProductModel requestModel)
    {
        try
        {
            var product = await _product.Update(id, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = product.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = product.Message,
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
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var product = await _product.Delete(id);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = product.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = product.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProductList(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var productLst = await _product.GetProduct(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = productLst.DataLst.Count,
                IsSuccess = productLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Product,
                Message = productLst.MessageResponse.Message,
                Item = productLst.DataLst,
                PageSetting = productLst.PageSetting
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}