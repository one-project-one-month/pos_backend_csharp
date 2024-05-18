using DotNet8.PosBackendApi.Models;
using DotNet8.PosFrontendBlazor.Models;
using DotNet8.PosFrontendBlazor.Models.Report;
using Radzen;
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
        private async Task DateChanged(DateTime? newDate)
        {
            DateValue = newDate;
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
        }
    }
}
