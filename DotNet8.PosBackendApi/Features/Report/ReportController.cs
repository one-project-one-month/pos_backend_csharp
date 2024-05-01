using Microsoft.AspNetCore.Mvc;

namespace DotNet8.PosBackendApi.Features.Report
{
    [Route("api/v1/report")]
    [ApiController]
    public class ReportController : BaseController
    {
        private readonly BL_Report _report;
        private readonly ResponseModel _response;
        private readonly JwtTokenGenerate _token;

        public ReportController(IServiceProvider serviceProvider, BL_Report report, ResponseModel response, JwtTokenGenerate token) : base(serviceProvider)
        {
            _report = report;
            _response = response;
            _token = token;
        }

        [Route("monthly-report")]
        [HttpGet]
        public async Task<IActionResult> MonthlyReport(int month, int year)
        {
            try
            {
                var lst = await _report.MonthlyReport(month, year);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = RefreshToken(),
                        Count = lst.Data.Count,
                        EnumPos = EnumPos.ProductCategory,
                        IsSuccess = lst.MessageResponse.IsSuccess,
                        Message = lst.MessageResponse.Message,
                        Item = lst.Data
                    });
                return Content(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("yearly-report")]
        [HttpGet]
        public async Task<IActionResult> YearlyReport(int year)
        {
            try
            {
                var lst = await _report.YearlyReport(year);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = RefreshToken(),
                        Count = lst.Data.Count,
                        EnumPos = EnumPos.ProductCategory,
                        IsSuccess = lst.MessageResponse.IsSuccess,
                        Message = lst.MessageResponse.Message,
                        Item = lst.Data
                    });
                return Content(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
