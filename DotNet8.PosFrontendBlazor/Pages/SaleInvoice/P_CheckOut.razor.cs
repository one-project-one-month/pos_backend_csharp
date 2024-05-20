using DotNet8.PosFrontendBlazor.Models.Product;
using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice
{
    public partial class P_CheckOut
    {
        private SaleInvoiceModel? reqModel = new SaleInvoiceModel();

        [Parameter]
        public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();

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
            reqModel.SaleInvoiceDetails = SaleInvoiceDetails;
            Console.WriteLine(JsonConvert.SerializeObject(reqModel).ToString());
            //saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;
        }

        //private Task Set(string value)
        //{
        //    reqModel.PaymentAmount = value == null ? 0 : Convert.ToDecimal(value);
        //    return Task.CompletedTask;
        //}
    }
}
