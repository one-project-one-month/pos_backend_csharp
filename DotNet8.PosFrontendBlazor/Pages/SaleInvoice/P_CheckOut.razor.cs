using DotNet8.PosFrontendBlazor.Models.Product;
using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice
{
    public partial class P_CheckOut
    {
        private SaleInvoiceModel? reqModel = new SaleInvoiceModel();

        [Parameter]
        public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; }

        public string MobileNo { get; set; }
        public string StaffName { get; set; }

        private void AddItem(ProductModel requestModel)
        {
            SaleInvoiceDetailModel saleInvoiceDetail = new SaleInvoiceDetailModel
            {
                ProductCode = requestModel.ProductCode,
                ProductName = requestModel.ProductName,
                Price = requestModel.Price,
            };

            if (!SaleInvoiceDetails.Where(x => x.ProductCode == requestModel.ProductCode).Any())
            {
                saleInvoiceDetail.Quantity = 1;
                saleInvoiceDetail.Amount = requestModel.Price;
                SaleInvoiceDetails!.Add(saleInvoiceDetail);
            }
            else
            {
                SaleInvoiceDetails.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity += 1;
                SaleInvoiceDetails.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount += requestModel.Price;
            }
            Console.WriteLine(SaleInvoiceDetails.Select(x => x.Price * x.Quantity).Sum());
        }

        private void IncreaseCount(SaleInvoiceDetailModel requestModel)
        {
            requestModel.Quantity += 1;
            SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity; ;
            SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
        }

        private void DecreaseCount(SaleInvoiceDetailModel requestModel, int quantity)
        {
            if (requestModel.Quantity > 0)
            {
                requestModel.Quantity -= quantity;
                SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity;
                SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
            }
        }

        private void Pay()
        {
            Console.WriteLine(JsonConvert.SerializeObject(SaleInvoiceDetails));
            //saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;
        }
    }
}
