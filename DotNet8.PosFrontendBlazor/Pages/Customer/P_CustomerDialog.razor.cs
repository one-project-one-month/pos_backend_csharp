using DotNet8.PosFrontendBlazor.Models.Customer;

namespace DotNet8.PosFrontendBlazor.Pages.Customer;

public partial class P_CustomerDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private CustomerRequestModel requestModel = new();

    [Parameter] public int CustomerId { get; set; }

    [Parameter] public string CustomerCode { get; set; } = null!;

    [Parameter] public string CustomerName { get; set; } = null!;

    [Parameter] public string MobileNo { get; set; } = null!;

    [Parameter] public DateTime? DateOfBirth { get; set; }

    [Parameter] public string Gender { get; set; } = null!;

    [Parameter] public string StateCode { get; set; } = null!;

    [Parameter] public string TownshipCode { get; set; } = null!;


    private void Cancel() => MudDialog?.Cancel();

    private async Task SaveAsync()
    {
        #region Validation
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
        #endregion

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

    private async Task UpdateAsync()
    {
        #region Validation

        if (string.IsNullOrEmpty(CustomerName))
        {
            ShowWarningMessage("Customer Name cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(MobileNo))
        {
            ShowWarningMessage("Mobile Number cannot be empty.");
            return;
        }

        if (MobileNo.Trim().Length > 11 || MobileNo.Trim().Length < 11)
        {
            ShowWarningMessage("Invalid Mobile Number.");
            return;
        }

        if (string.IsNullOrEmpty(Gender))
        {
            ShowWarningMessage("Gender is required.");
            return;
        }

        if (Gender == "0")
        {
            ShowWarningMessage("Invalid Gender.");
            return;
        }

        if (DateOfBirth is null)
        {
            ShowWarningMessage("Date Of Birth is required.");
            return;
        }


        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - Convert.ToDateTime(DateOfBirth);
        int age = Convert.ToInt32(ageDifference.TotalDays / 365);

        if (age <= 18 || age >= 40)
        {
            ShowWarningMessage("Age must be between 18 and 45.");
            return;
        }
        #endregion

        CustomerRequestModel requestModel = new()
        {
            CustomerName = CustomerName,
            MobileNo = MobileNo,
            DateOfBirth = DateOfBirth,
            Gender = Gender,
            StateCode = StateCode,
            TownshipCode = TownshipCode
        };

        var response = await HttpClientService.ExecuteAsync<CustomerResponseModel>(
            $"{Endpoints.Customer}/{CustomerId}",
            EnumHttpMethod.Patch,
            requestModel);

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }
}