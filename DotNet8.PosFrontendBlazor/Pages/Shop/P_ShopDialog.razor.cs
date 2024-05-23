using DotNet8.PosFrontendBlazor.Models.Product;
using DotNet8.PosFrontendBlazor.Models.Shop;

namespace DotNet8.PosFrontendBlazor.Pages.Shop
{
    public partial class P_ShopDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private ShopRequestModel RequestModel = new();

        void Cancel() => MudDialog?.Cancel();

        private async Task SaveAsync()
        {
            if (validate())
            {
                var response = await HttpClientService.ExecuteAsync<ShopResponseModel>(
                Endpoints.Shop,
                EnumHttpMethod.Post,
                RequestModel
            );
                if (response.IsError)
                {
                    InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                    return;
                }

                InjectService.ShowMessage(response.Message, EnumResponseType.Success);
                MudDialog.Close();
            }
        }
        private bool validate()
        {
            if (string.IsNullOrEmpty(RequestModel.ShopCode))
            {
                ShowWarningMessage("Shop Category Code is required.");
                return false;
            }
            if (string.IsNullOrEmpty(RequestModel.ShopName))
            {
                ShowWarningMessage("Shop Name is required.");
                return false;
            }
            if (string.IsNullOrEmpty(RequestModel.MobileNo))
            {
                ShowWarningMessage("Mobile Number is required.");
                return false;
            }
            if (string.IsNullOrEmpty(RequestModel.Address))
            {
                ShowWarningMessage("Address is required.");
                return false;
            }
            return true;
        }
        private void ShowWarningMessage(string message)
        {
            InjectService.ShowMessage(message, EnumResponseType.Warning);
        }
    }
}
