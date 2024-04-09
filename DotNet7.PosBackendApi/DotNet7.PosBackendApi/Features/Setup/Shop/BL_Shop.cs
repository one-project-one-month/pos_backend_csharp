namespace DotNet7.PosBackendApi.Features.Setup.Shop;

public class BL_Shop
{
    private readonly DL_Shop _dL_Shop;

    public BL_Shop(DL_Shop dL_Shop)
    {
        _dL_Shop = dL_Shop;
    }
    public async Task<List<ShopModel>> GetShops()
    {
        var shopList = await _dL_Shop.GetShops();   
        return shopList;
    }

    public async Task<ShopModel> GetShop(int id)
    {
        var shop = await _dL_Shop.GetShop(id);
        return shop;
    }

    public async Task<MessageResponseModel> CreateShop(ShopModel requestModel)
    {
        CheckShopNullValue(requestModel);
        var responseModel = await _dL_Shop.CreateShop(requestModel);
        return responseModel;

    }

    public async Task<MessageResponseModel> UpdateShop(int id, ShopModel requestModel)
    {
        if (id == 0) throw new Exception("id is 0.");
        CheckShopNullValue(requestModel);
        var responseModel = await _dL_Shop.UpdateShop(id, requestModel);
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteShop(int id)
    {
        if (id == 0) throw new Exception("id is 0.");
        var responseModel = await _dL_Shop.DeleteShop(id);
        return responseModel;
    }

    private static void CheckShopNullValue(ShopModel shop)
    {
        if (shop == null)
        {
            throw new Exception("shop is null.");
        }
        if (string.IsNullOrWhiteSpace(shop.ShopCode))
        {
            throw new Exception("shop.ShopCode shop is null.");
        }
        if (string.IsNullOrWhiteSpace(shop.ShopName))
        {
            throw new Exception("shop.ShopName shop is null.");
        }
        if (string.IsNullOrWhiteSpace(shop.MobileNo))
        {
            throw new Exception("shop.ShopName shop is null.");
        }
        if (string.IsNullOrWhiteSpace(shop.Address))
        {
            throw new Exception("shop.ShopName shop is null.");
        }
    }
}