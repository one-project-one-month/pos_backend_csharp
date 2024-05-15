using DotNet8.PosFrontendBlazor.Models.Tax;

namespace DotNet8.PosFrontendBlazor.Pages.Tax;

public partial class P_CreateTaxDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    public bool showPercentageField = false;

    public bool showFixedAmountField = false;

    TaxModel requestModel = new();

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

        if (requestModel.Percentage != 0 && requestModel.Percentage is not null)
        {
            if (requestModel.Percentage == 100 || requestModel.Percentage > 100)
            {
                InjectService.ShowMessage("Percentage is invalid.", EnumResponseType.Warning);
                return;
            }
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
            Endpoints.Tax,
            EnumHttpMethod.Post,
            requestModel);

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private void TaxTypeChanged(string tax)
    {
        if (tax == nameof(EnumTaxType.Percentage))
        {
            showPercentageField = true;
            showFixedAmountField = false;
        }
        else
        {
            showFixedAmountField = true;
            showPercentageField = false;
        }
    }

    public enum EnumTaxType
    {
        None,
        Percentage,
        Fixed
    }
}
