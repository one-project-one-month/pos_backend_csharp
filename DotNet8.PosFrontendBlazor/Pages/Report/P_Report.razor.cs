using DotNet8.PosBackendApi.Models;
using DotNet8.PosFrontendBlazor.Models;
using DotNet8.PosFrontendBlazor.Models.Report;
using Radzen;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DotNet8.PosFrontendBlazor.Pages.Report
{
    public partial class P_Report
    {
        private bool _editable = true;
        private bool _clearable = true;
        private DateRange _dateRange { get; set; }

        private int _pageNo = 1;

        private int _pageSize = 10;

        private ReportListResponseModel? responseModel;
        private EnumReportDate DateFormat { get; set; }
        private DateTime? DateValue { get; set; }
        private string? fromDate {  get; set; }
        private string? toDate { get; set; }
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

        private async Task DateRangeChanged(DateRange? newDateRange)
        {
            fromDate = newDateRange?.Start?.ToString("yyyy-MMM-dd");
            toDate = newDateRange?.End?.ToString("yyyy-MMM-dd");
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
        }

        private async Task ReportMonthly()
        {
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/monthly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
        }

        private async Task ReportYearly()
        {
            responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/yearly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
            );
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
