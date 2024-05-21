using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice
{
    public partial class P_CheckOut
    {
        

        [Parameter]
        public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();

        private SaleInvoiceModel? reqModel = new SaleInvoiceModel();

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

        private async void Pay()
        {
            reqModel.SaleInvoiceDetails = SaleInvoiceDetails;
            reqModel.SaleInvoiceDateTime = DateTime.Now;
            reqModel.TotalAmount = SaleInvoiceDetails.Sum(x=> x.Amount);
            reqModel.StaffCode = "S_001";
            reqModel.PaymentType = "KBZPay";
            Console.WriteLine(JsonConvert.SerializeObject(reqModel).ToString());
            var response = await HttpClientService.ExecuteAsync<SaleInvoiceResponseModel>(
                Endpoints.SaleInvoice,
                EnumHttpMethod.Post,
                reqModel
                );
            Console.WriteLine(JsonConvert.SerializeObject(response).ToString());
            if (response.IsError)
            {
                InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                return;
            }

            InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        }
    }
}
