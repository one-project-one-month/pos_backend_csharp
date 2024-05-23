using DotNet8.PosBackendApi.Models;
using DotNet8.PosFrontendBlazor.Models;
using DotNet8.PosFrontendBlazor.Models.Report;
using Radzen;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DotNet8.PosFrontendBlazor.Pages.Report
{
    public partial class P_Report
    {
        private int _pageNo = 1;
        private int _pageSize = 10;
        private int? _dateDay;
        private int? _dateMonth;
        private int? _dateYear;
        private ReportListResponseModel? responseModel;
        private EnumReportDate DateFormat { get; set; }
        private DateTime? DateValue { get; set; }
        private string? fromDate {  get; set; }
        private string? toDate { get; set; }
        private string? CheckValue { get; set; }
        private async Task FromDateChanged(DateTime? newDate)
        {
            fromDate = newDate?.ToString("yyyy-MMM-dd") ?? DateTime.Today.ToString("yyyy-MMM-dd");
            toDate = !string.IsNullOrEmpty(toDate) ? toDate : DateTime.Today.ToString("yyyy-MMM-dd");
            await OnValueChanged();
        }

        private async Task ToDateChanged(DateTime? newDate)
        {
            toDate = newDate?.ToString("yyyy-MMM-dd") ?? DateTime.Today.ToString("yyyy-MMM-dd");
            fromDate = !string.IsNullOrEmpty(fromDate) ? fromDate : DateTime.Today.ToString("yyyy-MMM-dd");
            await OnValueChanged();
        }

        private async Task PageChanged(int i)
        {
            _pageNo = i;
            await OnValueChanged();
        }
        private async Task OnValueChanged()
        {
            await InjectService.EnableLoading();
            switch (DateFormat)
            {
                case EnumReportDate.Daily:
                    await ReportDaily();
                    break;
                case EnumReportDate.Monthly:
                    await ReportMonthly();
                    break;
                case EnumReportDate.Yearly:
                    await ReportYearly();
                    break;
                case EnumReportDate.None:
                    responseModel = null;
                    break;
            }
            StateHasChanged();
            await InjectService.DisableLoading();
        }

        private async Task ReportDaily()
        {
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/daily-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
            CheckValue = $"{Endpoints.Report}/daily-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}";
        }
        private async Task ReportMonthly()
        {
            _dateMonth = DateValue?.Month;
            _dateYear = DateValue?.Year;
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/monthly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
            CheckValue = $"{Endpoints.Report}/monthly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}";
        }
        private async Task ReportYearly()
        {
            _dateMonth = DateValue?.Month;
            _dateYear = DateValue?.Year;
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/yearly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
            CheckValue = $"{Endpoints.Report}/yearly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}";
        }

        /*private async Task ReportDaily()
        {
            _dateDay = DateValue?.Day;
            _dateMonth = DateValue?.Month;
            _dateYear = DateValue?.Year;
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/daily-report/{_dateDay}/{_dateMonth}/{_dateYear}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
        }
        private async Task ReportMonthly()
        {
            _dateMonth = DateValue?.Month;
            _dateYear = DateValue?.Year;
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/monthly-report/{_dateMonth}/{_dateYear}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
        }
        private async Task ReportYearly()
        {
            _dateMonth = DateValue?.Month;
            _dateYear = DateValue?.Year;
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/yearly-report/{_dateYear}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
        }*/
    }
}
