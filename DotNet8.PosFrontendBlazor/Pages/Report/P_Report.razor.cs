using DotNet8.PosBackendApi.Models;
using DotNet8.PosFrontendBlazor.Models;
using DotNet8.PosFrontendBlazor.Models.Report;
using Radzen;
namespace DotNet8.PosFrontendBlazor.Pages.Report
{
    public partial class P_Report
    {
        public ReportListResponseModel? responseModel;
        private string DateFormat { get; set; }
        private DateTime? DateValue { get; set; } = DateTime.Today;
        private int _pageNo = 1;
        private int _pageSize = 10;
        private async Task OnValueChanged()
        {
            int? dateDay = DateValue?.Day;
            int? dateMonth = DateValue?.Month;
            int? dateYear = DateValue?.Year;

            if (DateFormat.ToLower() is "daily")
            {
                await InjectService.EnableLoading();
                responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
                $"{Endpoints.Report}/daily-report/{dateDay}/{dateMonth}/{dateYear}/{_pageNo}/{_pageSize}",
                EnumHttpMethod.Get
                );
            }
            else if (DateFormat.ToLower() is "monthly")
            {
                await InjectService.EnableLoading();
                responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
                $"{Endpoints.Report}/monthly-report/{dateMonth}/{dateYear}",
                EnumHttpMethod.Get
                );
            }
            else if (DateFormat.ToLower() is "yearly")
            {
                await InjectService.EnableLoading();
                responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
                $"{Endpoints.Report}/yearly-report/{dateYear}",
                EnumHttpMethod.Get
                );
            }
            StateHasChanged();
            await InjectService.DisableLoading();
        }

        private async Task PageChanged(int i)
        {
            _pageNo = i;
            await OnValueChanged();
        }
    }
}
