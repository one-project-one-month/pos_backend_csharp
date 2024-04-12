namespace DotNet7.PosBackendApi.Features.Setup.Product;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : BaseController
{
    private readonly BL_Product _bL_Product;
    private readonly ResponseModel _response;

    public ProductController(BL_Product bL_Product, ResponseModel response)
    {
        _bL_Product = bL_Product;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetProduct()
    {
        try
        {
            var productLst = await _bL_Product.GetProduct();
            var responseModel = _response.ReturnGet
                (productLst.MessageResponse.Message,
                productLst.DataLst.Count,
                EnumPos.Product,
                productLst.MessageResponse.IsSuccess,
                productLst.DataLst);
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
            var product = await _bL_Product.GetProductByCode(productCode);
            var responseModel = _response.ReturnById
                (product.MessageResponse.Message,
                EnumPos.Product,
                product.MessageResponse.IsSuccess,
                product.Data);
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
            var product = await _bL_Product.Create(requestModel);
            var responseModel = _response.ReturnCommand
                (product.IsSuccess, product.Message, EnumPos.Product, requestModel);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductModel requestModel)
    {
        try
        {
            var product = await _bL_Product.Update(id, requestModel);
            var responseModel = _response.ReturnCommand
                (product.IsSuccess, product.Message, EnumPos.Product, requestModel);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var product = await _bL_Product.Delete(id);
            var responseModel = _response.ReturnCommand
                (product.IsSuccess, product.Message, EnumPos.Product);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}
