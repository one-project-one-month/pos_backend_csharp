using DotNet8.PosBackendApi.Models.Setup.Product;
using DotNet8.PosBackendApi.Models.Setup.ProductCategory;
using DotNet8.PosBackendApi.Models.Setup.Shop;
using DotNet8.PosBackendApi.Models.Setup.Staff;

namespace DotNet8.PosBackendApi.Models;

public static class ChangeModel
{
    #region Shop

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

    #endregion

    #region Staff

    public static StaffModel Change(this TblStaff dataModel)
    {
        var staffModel = new StaffModel()
        {
            StaffId = dataModel.StaffId,
            StaffCode = dataModel.StaffCode,
            DateOfBirth = dataModel.DateOfBirth,
            MobileNo = dataModel.MobileNo,
            Address = dataModel.Address,
            Gender = dataModel.Gender,
            Position = dataModel.Gender
        };
        return staffModel;
    }

    public static TblStaff Change(this StaffModel requestModel)
    {
        var staffModel = new TblStaff()
        {
            // StaffId = requestModel.StaffId,
            StaffCode = requestModel.StaffCode,
            DateOfBirth = requestModel.DateOfBirth,
            MobileNo = requestModel.MobileNo,
            Address = requestModel.Address,
            Gender = requestModel.Gender,
            Position = requestModel.Gender
        };
        return staffModel;
    }

    #endregion

    #region Product
    public static ProductModel Change(this TblProduct dataModel)
    {
        var model = new ProductModel()
        {
            ProductId = dataModel.ProductId,
            ProductCategoryCode = dataModel.ProductCategoryCode,
            ProductCode = dataModel.ProductCode,
            ProductName = dataModel.ProductName,
            Price = dataModel.Price,
        };
        return model;
    }

    public static TblProduct Change(this ProductModel requestModel)
    {
        var model = new TblProduct()
        {
            ProductCategoryCode = requestModel.ProductCategoryCode,
            ProductCode = requestModel.ProductCode,
            ProductName = requestModel.ProductName,
            Price = requestModel.Price,
        };
        return model;
    }
    #endregion

    #region ProductCategory

    public static ProductCategoryModel Change(this TblProductCategory dataModel)
    {
        var model = new ProductCategoryModel()
        {
            ProductCategoryId = dataModel.ProductCategoryId,
            ProductCategoryCode = dataModel.ProductCategoryCode,
            ProductCategoryName = dataModel.ProductCategoryName,
        };
        return model;
    }

    public static TblProductCategory Change(this ProductCategoryModel dataModel)
    {
        var model = new TblProductCategory()
        {
            ProductCategoryId = dataModel.ProductCategoryId,
            ProductCategoryCode = dataModel.ProductCategoryCode,
            ProductCategoryName = dataModel.ProductCategoryName,
        };
        return model;
    }

    #endregion
}