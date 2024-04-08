using DotNet7.PosBackendApi.DbService.DbModels;
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
            var shopList = await _context.TblShops.ToListAsync();
            return shopList.Select(x => new ShopModel
            {
                ShopId = x.ShopId,
                ShopCode = x.ShopCode,
                ShopName = x.ShopName,
                MobileNo = x.MobileNo,
                Address = x.Address
            }).ToList();
        }

        public async Task<ShopModel> GetShop(int id)
        {
            var shopObj = await _context.TblShops.FirstOrDefaultAsync(x => x.ShopId == id);
            if (shopObj != null)
            {
                return new ShopModel()
                {
                    ShopId = shopObj.ShopId,
                    ShopCode = shopObj.ShopCode,
                    ShopName = shopObj.ShopName,
                    MobileNo = shopObj.MobileNo,
                    Address = shopObj.Address
                };
            }
            return new ShopModel();
        }

        public async Task<string> CreateShop(ShopModel shop)
        {
            string message = "Null Object";

            if (shop != null)
            {
                TblShop shopObj = new TblShop()
                {
                    ShopId = shop.ShopId,
                    ShopCode = shop.ShopCode,
                    ShopName = shop.ShopName,
                    MobileNo = shop.MobileNo,
                    Address = shop.Address
                };
                _context.TblShops.Add(shopObj);

                var result = _context.SaveChanges();
                message = result > 0 ? "Save Successful" : "Unsuccessful";
                return await Task.FromResult(message);
            }
            return await Task.FromResult(message);

        }

        public async Task<string> UpdateShop(int id, ShopModel shop)
        {
            string message = "Null Object";
            var shopObj = await _context.TblShops.FirstOrDefaultAsync(x => x.ShopId == id);

            if (shopObj != null)
            {
                shopObj.ShopId = id;
                shopObj.ShopCode = shop.ShopCode;
                shopObj.ShopName = shop.ShopName;
                shopObj.MobileNo = shop.MobileNo;
                shopObj.Address = shop.Address;
                var result = _context.SaveChanges();
                message = result > 0 ? "Update Successful" : "Update Unsuccessful";
                return await Task.FromResult(message);
            }

            message = "Shop Not Found";
            return await Task.FromResult(message);

        }

        public async Task<string> DeletShop(int id)
        {
            string message = "Null Object";
            var ShopObj = await _context.TblShops.FirstOrDefaultAsync(x => x.ShopId == id);
            if (ShopObj != null)
            {
                _context.Remove(ShopObj);
                var result = _context.SaveChanges();
                message = result > 0 ? "Delete Shop Successful" : "Unsuccessful Delete Shop";
                return await Task.FromResult(message);
            }

            message = "Shop Not Found";
            return await Task.FromResult(message);

        }
    }
}
