namespace DotNet8.PosBackendApi.Features.Report
{
    public class BL_Report
    {
        private readonly DL_Report _report;

        public BL_Report(DL_Report report) => _report = report;

        public async Task<MonthlyReportResponseModel> DailyReport(int DateDay, int DateMonth, int DateYear, int PageNo,int PageSize)
        {
            var response = await _report.DailyReport(DateDay, DateMonth, DateYear, PageNo, PageSize);
            return response;
        }

        public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year, int PageNo, int PageSize)
        {
            var response = await _report.MonthlyReport(month, year, PageNo, PageSize);
            return response;
        }

        public async Task<MonthlyReportResponseModel> YearlyReport(int year, int PageNo, int PageSize)
        {
            var response = await _report.YearlyReport(year, PageNo, PageSize);
            return response;
        }
    }
}
