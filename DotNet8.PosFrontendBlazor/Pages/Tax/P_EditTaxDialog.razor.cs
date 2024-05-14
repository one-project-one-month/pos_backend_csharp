using DotNet8.PosFrontendBlazor.Models.Tax;

namespace DotNet8.PosFrontendBlazor.Pages.Tax;

public partial class P_EditTaxDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public TaxModel requestModel { get; set; } = new();

    void Cancel() => MudDialog?.Cancel();

    public async Task SaveAsync()
    {
        #region Validation

        if (requestModel.FromAmount <= 0 || requestModel.FromAmount is null)
        {
            InjectService.ShowMessage("From Amount is invalid.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.ToAmount <= 0 || requestModel.ToAmount is null)
        {
            InjectService.ShowMessage("To Amount is invalid.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.Percentage == 100 || requestModel.Percentage > 100 || requestModel.Percentage is null)
        {
            InjectService.ShowMessage("Percentage is invalid.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.FromAmount >= requestModel.ToAmount)
        {
            InjectService.ShowMessage("From Amount must be less than To Amount", EnumResponseType.Warning);
            return;
        }

        #endregion

        requestModel.FromAmount = Convert.ToInt32(requestModel.FromAmount);
        requestModel.ToAmount = Convert.ToInt32(requestModel.ToAmount);
        requestModel.Percentage = Convert.ToInt32(requestModel.Percentage);

        var response = await HttpClientService.ExecuteAsync<TaxResponseModel>(
            $"{Endpoints.Tax}/{requestModel.TaxId}",
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
}
