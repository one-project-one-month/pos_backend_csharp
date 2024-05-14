using DotNet8.PosFrontendBlazor.Models.Product;

namespace DotNet8.PosFrontendBlazor.Pages.Product
{
    public partial class P_ProductEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public ProductRequestModel reqModel { get; set; } = new();
        void Cancel() => MudDialog?.Cancel();
        private async Task SaveAsync()
        {
            if (validate())
            {
                var response = await HttpClientService.ExecuteAsync<ProductResponseModel>(
                Endpoints.Product + $"/{reqModel.ProductId}",
                EnumHttpMethod.Patch,
                reqModel
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
            if (string.IsNullOrEmpty(reqModel.ProductName))
            {
                ShowWarningMessage("Product Name is required.");
                return false;
            }
            if (string.IsNullOrEmpty(reqModel.ProductCategoryCode))
            {
                ShowWarningMessage("Product Category Code is required.");
                return false;
            }
            if (!(reqModel.Price > 0))
            {
                ShowWarningMessage("Product Price must be greater than zero.");
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
