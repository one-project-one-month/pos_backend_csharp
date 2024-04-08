using DotNet7.PosBackendApi.DbService.DbModels;
using DotNet7.PosBackendApi.Models;
using DotNet7.PosBackendApi.Models.Setup.Shop;
using DotNet7.PosBackendApi.Models.Setup.Staff;
using Microsoft.EntityFrameworkCore;

namespace DotNet7.PosBackendApi.Features.Setup.Shop
{
    public class ShopService
    {
        private readonly AppDbContext _context;

        public ShopService(AppDbContext context)
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

        public async Task<string> CreateShop(ShopModel requestModel)
        {
            string message = "";
            CheckShopNullValue(requestModel);
            await _context.TblShops.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            message = result > 0 ? "Successfully Save." : "Fail To Save.";
            return message;

        }

        public async Task<string> UpdateShop(int id, ShopModel requestModel)
        {
            string message = "Shop Not Found";

            if (id == 0) throw new Exception("id is 0.");
            var shopObj = await _context
                .TblShops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShopId == id);
            if (shopObj == null)
            {
                //throw new Exception("ShopModel shop is null");
                goto result;
            }
            CheckShopNullValue(requestModel);
            await _context.TblShops.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            message = result > 0 ? "Successfully Update." : "Fail To Update.";
        result:
            return message;
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

        public async Task<string> DeleteShop(int id)
        {
            string message = "Shop Not Found";
            if(id == 0) throw new Exception("id is 0.");
            var shop = await _context
                .TblShops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShopId == id);
            if (shop == null)
            {
                goto result;
            }
            _context.Remove(shop);
            var result = await _context.SaveChangesAsync();
            message = result > 0 ? "Successfully Delete." : "Fail To Delete.";
        result:
            return message;
        }
    }
}
