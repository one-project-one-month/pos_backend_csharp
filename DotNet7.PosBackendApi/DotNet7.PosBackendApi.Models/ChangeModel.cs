using DotNet7.PosBackendApi.DbService.DbModels;
using DotNet7.PosBackendApi.Models.Setup.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet7.PosBackendApi.Models
{
    public static class ChangeModel
    {
        public static TblShop Change(this ShopModel requestModel)
        {
            var shopModel = new TblShop()
            {
                //ShopId = shop.ShopId,
                ShopCode = requestModel.ShopCode,
                ShopName = requestModel.ShopName,
                MobileNo = requestModel.MobileNo,
                Address = requestModel.Address
            };
            return shopModel;
        }
        public static ShopModel Change(this TblShop dataModel)
        {
            var shopModel = new ShopModel()
            {
                ShopId = dataModel.ShopId,
                ShopCode = dataModel.ShopCode,
                ShopName = dataModel.ShopName,
                MobileNo = dataModel.MobileNo,
                Address = dataModel.Address
            };
            return shopModel;
        }
    }
}
