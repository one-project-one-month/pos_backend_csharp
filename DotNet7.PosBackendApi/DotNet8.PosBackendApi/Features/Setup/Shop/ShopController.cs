using DotNet8.PosBackendApi.Models.Setup.Shop;

namespace DotNet8.PosBackendApi.Features.Setup.Shop;

[Route("api/v1/shops")]
[ApiController]
public class ShopController : BaseController
{
    private readonly ShopService _shopService;
    private readonly BL_Shop _bL_Shop;
    private readonly ResponseModel _response;

    public ShopController(ShopService shopService, BL_Shop bL_Shop, ResponseModel response)
    {
        _shopService = shopService;
        _bL_Shop = bL_Shop;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetShops()
    {
        try
        {
            var shopLst =  await _bL_Shop.GetShops();
            var responseModel = _response.ReturnGet
                (shopLst.MessageResponse.Message,
                shopLst.DataLst.Count,
                EnumPos.Shop,
                shopLst.MessageResponse.IsSuccess,
                shopLst.DataLst);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShop(int id)
    {
        try
        {
            var shop = await _bL_Shop.GetShop(id);
            var responseModel = _response.ReturnById
                (shop.MessageResponse.Message,
                EnumPos.Shop, 
                shop.MessageResponse.IsSuccess,
                shop.Data);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateShop(ShopModel shop)
    {
        try
        {
            var model = await _bL_Shop.CreateShop(shop);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message,EnumPos.Shop,shop);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShop(int id, ShopModel shop)
    {
        try
        {
            var model = await _bL_Shop.UpdateShop(id, shop);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message, EnumPos.Shop, shop);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShop(int id)
    {
        try
        {
            var model = await _bL_Shop.DeleteShop(id);
            var responseModel = _response.ReturnCommand
                (model.IsSuccess, model.Message, EnumPos.Shop);
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }
}