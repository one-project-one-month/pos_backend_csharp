using DotNet8.PosFrontendBlazor.Models.Dashboard;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.Dashboard
{
    public partial class P_Dashboard
    {
        private DashboardRequestModel? _requestModel { get; set; } = new DashboardRequestModel();
        public DashboardResponseModel _responseModel { get; set; } = new DashboardResponseModel();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _requestModel.SaleInvoiceDate = DateTime.Now;
                _responseModel = await HttpClientService.ExecuteAsync<DashboardResponseModel>($"{Endpoints.Dashboard}", EnumHttpMethod.Post, _requestModel);
                Console.WriteLine(JsonConvert.SerializeObject(_responseModel).ToString());
                StateHasChanged();

                var productName = _responseModel.Data.Dashboard.BestSellerProduct.Select(b => b.ProductName).ToList().ToArray();
                var quantity = _responseModel.Data.Dashboard.BestSellerProduct.Select(b => b.TotalQty).ToList().ToArray();
                var response = new
                {
                    productName = productName,
                    quantity = quantity
                };

                var DailySaleInvoiceDate = _responseModel.Data.Dashboard.WeeklyData.Select(b => b.SaleInvoiceDate.ToString("dd/MM/yyyy")).ToList().ToArray();
                var TotalAmt = _responseModel.Data.Dashboard.WeeklyData.Select(b => b.Amount).ToList().ToArray();
                var dailyResponse = new
                {
                    SalesInvoiceDate = DailySaleInvoiceDate,
                    TotalAmount = TotalAmt
                };
                
                await InjectService.EnableLoading();
                await JSRuntime.InvokeVoidAsync("setLineColumnChart", response);
                await JSRuntime.InvokeVoidAsync("setFunnelChart", dailyResponse);
                await InjectService.DisableLoading();
            }
        }
    }
}
