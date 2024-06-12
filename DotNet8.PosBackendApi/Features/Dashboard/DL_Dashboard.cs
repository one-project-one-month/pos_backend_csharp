namespace DotNet8.PosBackendApi.Features.Dashboard;

public class DL_Dashboard
{
    private readonly AppDbContext _context;
    private readonly DapperService _dapperService;
    public DL_Dashboard(AppDbContext context, DapperService dapperService)
    {
        _context = context;
        _dapperService = dapperService;
    }

    public async Task<DashboardResponseModel> Dashboard(DashboardRequestModel requestModel)
    {
        DashboardResponseModel responseModel = new DashboardResponseModel();
        try
        {
            var parameters = new { SaleInvoiceDate = requestModel.SaleInvoiceDate };
            var result = await _dapperService.QueryMultipleAsync<
                BestSellerProductDashboardModel,
                DailyDashboardModel,
                WeeklyDashboardModel,
                MonthlyDashboardModel,
                YearlyDashboardModel>("sp_Dashboard", parameters);
            if (result.Item5 is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.BestProductData = result.Item1.ToList();
            responseModel.DailyData = result.Item2.ToList();
            responseModel.WeeklyData = result.Item3.ToList();
            responseModel.MonthlyData = result.Item4.ToList();
            responseModel.YearlyData = result.Item5.ToList();
            responseModel.MessageResponse = responseModel.YearlyData.Count > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
            return responseModel;
        }
        catch (Exception ex)
        {
            responseModel.BestProductData = new List<BestSellerProductDashboardModel>();
            responseModel.DailyData = new List<DailyDashboardModel>();
            responseModel.WeeklyData = new List<WeeklyDashboardModel>();
            responseModel.MonthlyData = new List<MonthlyDashboardModel>();
            responseModel.YearlyData = new List<YearlyDashboardModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }

        result:
        return responseModel;
    }
}