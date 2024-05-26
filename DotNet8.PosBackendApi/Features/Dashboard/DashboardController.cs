using Azure;
using Microsoft.AspNetCore.Http;

namespace DotNet8.PosBackendApi.Features.Dashboard
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly ResponseModel _response;
        private readonly BL_Dashboard _dashboard;

        public DashboardController(IServiceProvider serviceProvider, BL_Dashboard dashboard, ResponseModel response) : base(serviceProvider)
        {
            _dashboard = dashboard;
            _response = response;
        }

        [HttpPost("dashboard")]
        public async Task<IActionResult> Dashboard(DashboardRequestModel requestModel)
        {
            try
            {
                var responseModel = await _dashboard.Dashboard(requestModel);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = RefreshToken(),
                        EnumPos = EnumPos.SaleInvoice,
                        IsSuccess = responseModel.MessageResponse.IsSuccess,
                        Message = responseModel.MessageResponse.Message,
                        //BestSellerProduct = responseModel.BestProductData,
                        //DailyData = responseModel.DailyData,
                        //WeeklyData = responseModel.WeeklyData,
                        //MonthlyData = responseModel.MonthlyData,
                        //YearlyData = responseModel.YearlyData
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
