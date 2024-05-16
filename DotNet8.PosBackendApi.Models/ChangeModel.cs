using DotNet8.PosBackendApi.Models.Setup.Customer;
using DotNet8.PosBackendApi.Models.Setup.Product;
using DotNet8.PosBackendApi.Models.Setup.ProductCategory;
using DotNet8.PosBackendApi.Models.Setup.SaleInvoice;
using DotNet8.PosBackendApi.Models.Setup.Shop;
using DotNet8.PosBackendApi.Models.Setup.Staff;
using DotNet8.PosBackendApi.Models.Setup.State;
using DotNet8.PosBackendApi.Models.Setup.Tax;
using DotNet8.PosBackendApi.Models.Setup.Township;

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
            StaffName = dataModel.StaffName,
            DateOfBirth = dataModel.DateOfBirth,
            MobileNo = dataModel.MobileNo,
            Address = dataModel.Address,
            Gender = dataModel.Gender,
            Position = dataModel.Position
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
            Position = requestModel.Position,
            StaffName = requestModel.StaffName,
            Password = requestModel.Password,
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

    #region Sale Invoice

    public static SaleInvoiceModel Change(this TblSaleInvoice dataModel)
    {
        var model = new SaleInvoiceModel()
        {
            SaleInvoiceId = dataModel.SaleInvoiceId,
            SaleInvoiceDateTime = dataModel.SaleInvoiceDateTime,
            VoucherNo = dataModel.VoucherNo,
            TotalAmount = dataModel.TotalAmount,
            Discount = dataModel.Discount,
            StaffCode = dataModel.StaffCode,
            Tax = dataModel.Tax,
            PaymentType = dataModel.PaymentType,
            CustomerAccountNo = dataModel.CustomerAccountNo,
            PaymentAmount = dataModel.PaymentAmount,
            ReceiveAmount = dataModel.ReceiveAmount,
            Change = dataModel.Change,
            CustomerCode = dataModel.CustomerCode
        };
        return model;
    }

    public static TblSaleInvoice Change(this SaleInvoiceModel dataModel)
    {
        var model = new TblSaleInvoice()
        {
            SaleInvoiceId = dataModel.SaleInvoiceId,
            SaleInvoiceDateTime = dataModel.SaleInvoiceDateTime,
            VoucherNo = dataModel.VoucherNo,
            TotalAmount = dataModel.TotalAmount,
            Discount = dataModel.Discount,
            StaffCode = dataModel.StaffCode,
            Tax = dataModel.Tax,
            PaymentType = dataModel.PaymentType,
            CustomerAccountNo = dataModel.CustomerAccountNo,
            PaymentAmount = dataModel.PaymentAmount,
            ReceiveAmount = dataModel.ReceiveAmount,
            Change = dataModel.Change,
            CustomerCode = dataModel.CustomerCode
        };
        return model;
    }

    #endregion

    #region Sale Invoice Detail

    public static SaleInvoiceDetailModel Change(this TblSaleInvoiceDetail dataModel)
    {
        var model = new SaleInvoiceDetailModel()
        {
            SaleInvoiceDetailId = dataModel.SaleInvoiceDetailId,
            VoucherNo = dataModel.VoucherNo,
            ProductCode = dataModel.ProductCode,
            Quantity = dataModel.Quantity,
            Price = dataModel.Price,
            Amount = dataModel.Amount
        };
        return model;
    }

    public static TblSaleInvoiceDetail Change(this SaleInvoiceDetailModel dataModel, string voucherNo)
    {
        var model = new TblSaleInvoiceDetail()
        {
            SaleInvoiceDetailId = dataModel.SaleInvoiceDetailId,
            //VoucherNo = dataModel.VoucherNo,
            VoucherNo = voucherNo,
            ProductCode = dataModel.ProductCode,
            Quantity = dataModel.Quantity,
            Price = dataModel.Price,
            Amount = dataModel.Amount
        };
        return model;
    }

    #endregion

    #region Sale Invoice & Sale Invoice Detail

    public static List<SaleInvoiceModel> Change(this List<TblSaleInvoice> lstInfo, List<TblSaleInvoiceDetail> lstDetail)
    {
        List<SaleInvoiceModel> responseModel = new List<SaleInvoiceModel>();
        responseModel = lstInfo.Select(x => new SaleInvoiceModel
        {
            SaleInvoiceId = x.SaleInvoiceId,
            SaleInvoiceDateTime = x.SaleInvoiceDateTime,
            VoucherNo = x.VoucherNo,
            TotalAmount = x.TotalAmount,
            Discount = x.Discount,
            StaffCode = x.StaffCode,
            Tax = x.Tax,
            PaymentType = x.PaymentType,
            CustomerAccountNo = x.CustomerAccountNo,
            PaymentAmount = x.PaymentAmount,
            ReceiveAmount = x.ReceiveAmount,
            Change = x.Change,
            CustomerCode = x.CustomerCode,
            SaleInvoiceDetails = lstDetail.Where(y => y.VoucherNo == x.VoucherNo).Select(z => new SaleInvoiceDetailModel
            {
                SaleInvoiceDetailId = z.SaleInvoiceDetailId,
                VoucherNo = z.VoucherNo,
                ProductCode = z.ProductCode,
                Quantity = z.Quantity,
                Price = z.Price,
                Amount = z.Amount
            }).ToList()
        }).ToList();
        return responseModel;
    }

    #endregion

    #region Customer

    public static CustomerModel Change(this TblCustomer dataModel)
    {
        var customerModel = new CustomerModel
        {
            CustomerId = dataModel.CustomerId,
            CustomerCode = dataModel.CustomerCode,
            CustomerName = dataModel.CustomerName,
            DateOfBirth = dataModel.DateOfBirth,
            Gender = dataModel.Gender,
            MobileNo = dataModel.MobileNo,
            StateCode = dataModel.StateCode,
            TownshipCode = dataModel.TownshipCode
        };
        return customerModel;
    }

    public static TblCustomer Change(this CustomerModel requestModel)
    {
        var customerDataModel = new TblCustomer
        {
            CustomerId = requestModel.CustomerId,
            CustomerCode = requestModel.CustomerCode,
            CustomerName = requestModel.CustomerName,
            DateOfBirth = requestModel.DateOfBirth,
            Gender = requestModel.Gender,
            MobileNo = requestModel.MobileNo,
            StateCode = requestModel.StateCode,
            TownshipCode = requestModel.TownshipCode
        };
        return customerDataModel;
    }

    #endregion

    #region Township

    public static TownshipModel Change(this TblPlaceTownship dataModel)
    {
        var township = new TownshipModel
        {
            TownshipId = dataModel.TownshipId,
            StateCode = dataModel.StateCode,
            TownshipCode = dataModel.TownshipCode,
            TownshipName = dataModel.TownshipName
        };
        return township;
    }

    public static TblPlaceTownship Change(this TownshipModel requestModel)
    {
        var townshipDataModel = new TblPlaceTownship
        {
            StateCode = requestModel.StateCode,
            TownshipCode = requestModel.TownshipCode,
            TownshipName = requestModel.TownshipName
        };
        return townshipDataModel;
    }

    #endregion

    #region State

    public static StateModel Change(this TblPlaceState dataModel)
    {
        var state = new StateModel
        {
            StateId = dataModel.StateId,
            StateCode = dataModel.StateCode,
            StateName = dataModel.StateName
        };
        return state;
    }

    public static TblPlaceState Change(this StateModel requestModel)
    {
        var StateDataModel = new TblPlaceState
        {
            StateCode = requestModel.StateCode,
            StateName = requestModel.StateName
        };
        return StateDataModel;
    }

    #endregion

    #region Tax

    public static TaxModel Change(this Tbl_Tax dataModel)
    {
        var tax = new TaxModel
        {
            TaxId = dataModel.TaxId,
            FromAmount = dataModel.FromAmount,
            ToAmount = dataModel.ToAmount,
            TaxType = dataModel.TaxType,
            Percentage = Convert.ToDecimal(dataModel.Percentage),
            FixedAmount = Convert.ToDecimal(dataModel.FixedAmount)
        };
        return tax;
    }

    public static Tbl_Tax Change(this TaxModel dataModel)
    {
        var tax = new Tbl_Tax
        {
            FromAmount = dataModel.FromAmount,
            ToAmount = dataModel.ToAmount,
            TaxType = dataModel.TaxType,
            Percentage = dataModel.Percentage,
            FixedAmount = dataModel.FixedAmount
        };
        return tax;
    }

    #endregion
}