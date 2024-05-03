namespace DotNet8.PosFrontendBlazor.Pages.Customer;

public partial class P_CustomerDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private CustomerRequestModel requestModel = new();

    private void Cancel() => MudDialog?.Cancel();

    private async Task SaveAsync()
    {
        Validate();

        var response = await HttpClientService.ExecuteAsync<CustomerResponseModel>(
            Endpoints.Customer,
            EnumHttpMethod.Post,
            requestModel
        );

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(requestModel.CustomerName))
        {
            ShowWarningMessage("Customer Name is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.MobileNo))
        {
            ShowWarningMessage("Mobile Number is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.Gender))
        {
            ShowWarningMessage("Gender is required.");
            return;
        }

        if (requestModel.DateOfBirth is null)
        {
            ShowWarningMessage("Date Of Birth is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.StateCode))
        {
            ShowWarningMessage("State Code is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.TownshipCode))
        {
            ShowWarningMessage("Township Code is required.");
            return;
        }
    }

    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }
}