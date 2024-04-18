using DotNet8.PosBackendApi.Models;
using DotNet8.PosBackendApi.Models.Setup.ProductCategory;

namespace DotNet8.PosBackendApi.Features.Setup.ProductCategory;
[Route("api/v1/[controller]")]
[ApiController]
public class ProductCategoryController : BaseController
{
    private readonly BL_ProductCategory _productCategory;
    private readonly ResponseModel _response;

    public ProductCategoryController(BL_ProductCategory productCategory, ResponseModel response)
    {
        _productCategory = productCategory;
        _response = response;
    }


    [HttpGet]
    public async Task<IActionResult> GetProductCategory()
    {
        try
        {
            var lst = await _productCategory.GetProductCategory();
            var model = _response.ReturnGet
                (lst.MessageResponse.Message,
                lst.DataList.Count,
                EnumPos.ProductCategory,
                lst.MessageResponse.IsSuccess,
                lst.DataList);
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{productCategoryCode}")]
    public async Task<IActionResult> GetProductCategoryByCode(string productCategoryCode)
    {
        try
        {
            var item = await _productCategory.GetProductCategoryByCode(productCategoryCode);
            var model = _response.ReturnById
                (item.MessageResponse.Message,
                EnumPos.ProductCategory,
                item.MessageResponse.IsSuccess,
                item.Data);
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCategoryModel requestModel)
    {
        try
        {
            var item = await _productCategory.CreateProductCategory(requestModel);
            var model = _response.ReturnCommand
                (item.IsSuccess, item.Message, EnumPos.ProductCategory, requestModel);
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductCategoryModel requestModel)
    {
        try
        {
            var item = await _productCategory.UpdateProductCategory(id, requestModel);
            var model = _response.ReturnCommand
                (item.IsSuccess, item.Message, EnumPos.ProductCategory, requestModel);
            return Content(model);
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
            var item = await _productCategory.DeleteProductCategory(id);
            var model = _response.ReturnCommand
                (item.IsSuccess, item.Message, EnumPos.ProductCategory);
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}
