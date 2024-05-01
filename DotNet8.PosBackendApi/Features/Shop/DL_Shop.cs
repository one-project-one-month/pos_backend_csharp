namespace DotNet8.PosBackendApi.Features.Shop;

public class DL_Shop
{
    private readonly AppDbContext _context;

    public DL_Shop(AppDbContext context) => _context = context;

    public async Task<ShopListResponseModel> GetShops()
    {
        var responseModel = new ShopListResponseModel();
        try
        {
            var shopList = await _context
                .TblShops
                .AsNoTracking()
                .ToListAsync();

            responseModel.DataLst = shopList.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ShopModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ShopResponseModel> GetShop(int id)
    {
        var responseModel = new ShopResponseModel();
        try
        {
            var shop = await _context
                .TblShops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShopId == id);
            if (shop is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = shop!.Change();
            responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        }
        catch (Exception ex)
        {
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateShop(ShopModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            await _context.TblShops.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateShop(int id, ShopModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var shop = await _context.TblShops.
                AsNoTracking().FirstOrDefaultAsync(x => x.ShopId == id);

            if (shop is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.ShopCode))
            {
                shop.ShopCode = requestModel.ShopCode;
            }

            if (!string.IsNullOrEmpty(requestModel.ShopName))
            {
                shop.ShopName = requestModel.ShopName;
            }

            if (!string.IsNullOrEmpty(requestModel.Address))
            {
                shop.Address = requestModel.Address;
            }

            if (!string.IsNullOrEmpty(requestModel.MobileNo))
            {
                shop.MobileNo = requestModel.MobileNo;
            }

            _context.Entry(shop).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteShop(int id)
    {
        var responseModel = new MessageResponseModel();
        try
        {
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
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }
}