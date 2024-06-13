namespace DotNet8.PosFrontendBlazor.Server.Components.Pages.Dashboard;

public partial class P_Dashboard
{
    private DashboardRequestModel? _requestModel { get; set; } = new DashboardRequestModel();
    public DashboardResponseModel _responseModel { get; set; } = new DashboardResponseModel();
    public string _yearlyDate { get; set; }
    public string _yearlyAmount { get; set; }
    public string _dailyDate { get; set; }
    public string _dailyAmount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _requestModel.SaleInvoiceDate = DateTime.Now;
        _responseModel = await HttpClientService.ExecuteAsync<DashboardResponseModel>($"{Endpoints.Dashboard}", EnumHttpMethod.Post, _requestModel);
        Console.WriteLine(JsonConvert.SerializeObject(_responseModel).ToString());
        StateHasChanged();

        if (_responseModel != null)
        {
            _yearlyDate = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Year.ToString() ?? string.Empty;
            _yearlyAmount = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
            _dailyDate = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.SaleInvoiceDate.ToString("dd-MM-yyyy") ?? string.Empty;
            _dailyAmount = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
            Console.WriteLine($"_yearlyDate{_yearlyDate} _yearlyAmount{_yearlyAmount} _dailyDate{_dailyDate} _dailyAmount{_dailyAmount}");
        }

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

        //await InjectService.EnableLoading();
        //await JSRuntime.InvokeVoidAsync("setLineColumnChart", response);
        //await JSRuntime.InvokeVoidAsync("setFunnelChart", dailyResponse);
        //await InjectService.DisableLoading();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _requestModel.SaleInvoiceDate = DateTime.Now;
            _responseModel = await HttpClientService.ExecuteAsync<DashboardResponseModel>($"{Endpoints.Dashboard}", EnumHttpMethod.Post, _requestModel);
            Console.WriteLine(JsonConvert.SerializeObject(_responseModel).ToString());
            StateHasChanged();

            if (_responseModel != null)
            {
                _yearlyDate = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Year.ToString() ?? string.Empty;
                _yearlyAmount = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
                _dailyDate = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.SaleInvoiceDate.ToString("dd-MM-yyyy") ?? string.Empty;
                _dailyAmount = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
                Console.WriteLine($"_yearlyDate{_yearlyDate} _yearlyAmount{_yearlyAmount} _dailyDate{_dailyDate} _dailyAmount{_dailyAmount}");
            }

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