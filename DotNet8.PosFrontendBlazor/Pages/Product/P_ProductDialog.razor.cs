using DotNet8.PosFrontendBlazor.Models.Product;

namespace DotNet8.PosFrontendBlazor.Pages.Product
{
    public partial class P_ProductDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private ProductRequestModel reqModel = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await HttpClientService.ExecuteAsync<ProductResponseModel>(
                Endpoints.Product,
                EnumHttpMethod.Post,
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
}
