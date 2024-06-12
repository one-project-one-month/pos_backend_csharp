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
        var response = await _dashboard.Dashboard(requestModel);
        return response;
    }
}