namespace DotNet8.PosBackendApi.Features.Dashboard;

public class BL_Dashboard
{
    private readonly DL_Dashboard _dashboard;

    public BL_Dashboard(DL_Dashboard dashboard)
    {
        _dashboard = dashboard;
    }

    public async Task<DashboardResponseModel> Dashboard(DashboardRequestModel requestModel)
    {
        if (requestModel.SaleInvoiceDate == default(DateTime)) 
            throw new Exception("Datetime is null");
        var response = await _dashboard.Dashboard(requestModel);
        return response;
    }
}