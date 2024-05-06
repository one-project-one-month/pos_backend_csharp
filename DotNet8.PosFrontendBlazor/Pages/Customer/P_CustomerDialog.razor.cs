using DotNet8.PosFrontendBlazor.Models.Customer;

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

        if (requestModel.MobileNo.Trim().Length > 11 || requestModel.MobileNo.Trim().Length < 11)
        {
            ShowWarningMessage("Invalid Mobile Number.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.Gender))
        {
            ShowWarningMessage("Gender is required.");
            return;
        }

        if (requestModel.Gender == "0")
        {
            ShowWarningMessage("Invalid Gender.");
            return;
        }

        if (requestModel.DateOfBirth is null)
        {
            ShowWarningMessage("Date Of Birth is required.");
            return;
        }


        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - Convert.ToDateTime(requestModel.DateOfBirth);
        int age = Convert.ToInt32(ageDifference.TotalDays / 365);

        if (age <= 18 || age >= 40)
        {
            ShowWarningMessage("Age must be between 18 and 45.");
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