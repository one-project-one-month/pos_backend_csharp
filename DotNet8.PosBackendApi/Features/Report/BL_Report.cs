using DotNet8.PosBackendApi.Models.Setup.Report;

namespace DotNet8.PosBackendApi.Features.Report
{
    public class BL_Report
    {
        private readonly DL_Report _report;

        public BL_Report(DL_Report report)
        {
            _report = report;
        }

        public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year)
        {
            var response = await _report.MonthlyReport(month, year);
            return response;
        }
    }
}
