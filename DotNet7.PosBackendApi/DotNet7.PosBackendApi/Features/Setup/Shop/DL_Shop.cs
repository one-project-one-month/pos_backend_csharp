namespace DotNet7.PosBackendApi.Features.Setup.Shop;

public class DL_Shop
{
    private readonly AppDbContext _context;

    public DL_Shop(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ShopModel>> GetShops()
    {
        var shopList = await _context
            .TblShops
            .AsNoTracking()
            .ToListAsync();
        return shopList.Select(x => x.Change()).ToList();
    }

    public async Task<ShopModel> GetShop(int id)
    {
        var shop = await _context
            .TblShops
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShopId == id);
        var responseModel = shop is not null ? shop.Change() : new ShopModel();
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateShop(ShopModel requestModel)
    {
        await _context.TblShops.AddAsync(requestModel.Change());
        var result = await _context.SaveChangesAsync();
        var responseModel = result > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateShop(int id, ShopModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        var shop = await _context
            .TblShops
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShopId == id);
        if (shop == null)
        {
            //throw new Exception("ShopModel shop is null");
            responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
            goto result;
        }

        await _context.TblShops.AddAsync(requestModel.Change());
        var result = await _context.SaveChangesAsync();
        responseModel = result > 0
            ? new MessageResponseModel(false, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteShop(int id)
    {
        var responseModel = new MessageResponseModel();
        var shop = await _context
            .TblShops
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShopId == id);
        if (shop == null)
        {
            responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
            goto result;
        }

        _context.Remove(shop);
        var result = await _context.SaveChangesAsync();
        responseModel = result > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        result:
        return responseModel;
    }
}