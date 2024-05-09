using DotNet8.PosFrontendBlazor.Models.Customer;

namespace DotNet8.PosFrontendBlazor.Pages.Customer;

public partial class P_Customer
{
    private CustomerListResponseModel? ResponseModel;

    private int _pageNo = 1;
    private int _pageSize = 10;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await List();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }

    private async Task List()
    {
        ResponseModel = await HttpClientService.ExecuteAsync<CustomerListResponseModel>(
            $"{Endpoints.Customer}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task Popup()
    {
        DialogResult result = await InjectService.ShowModalBoxAsync<P_CustomerDialog>("New Customer");

        if (!result.Canceled)
            await List();
    }

    private async Task EditPopup(int CustomerId, string CustomerName, string MobileNo, DateTime? DateOfBirth, string Gender, string StateCode, string TownshipCode)
    {
        CustomerParamsModel model = new(CustomerId, CustomerName, MobileNo, DateOfBirth, Gender, StateCode, TownshipCode);

        var parameters = new DialogParameters<P_CustomerDialog>();
        parameters.Add(x => x.model, model);

        DialogResult result = await InjectService.ShowModalBoxAsync<P_CustomerDialog>("Edit Customer", parameters);

        if (!result.Canceled)
            await List();
    }

    private async Task DeletePopUp(int id)
    {
        var parameters = new DialogParameters<P_DeleteCustomerDialog>()
        {
            {"id", id}
        };
        DialogResult result = await InjectService.ShowModalBoxAsync<P_DeleteCustomerDialog>("Delete Customer", parameters);

        if (!result.Canceled)
            await List();
    }

    private async Task PageChanged(int i)
    {
        _pageNo = i;
        await List();
    }
}
