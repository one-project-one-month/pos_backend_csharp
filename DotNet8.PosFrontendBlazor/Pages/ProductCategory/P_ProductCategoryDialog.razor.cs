namespace DotNet8.PosFrontendBlazor.Pages.ProductCategory;

public partial class P_ProductCategoryDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private ProductCategoryRequestModel reqModel = new();

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveAsync()
    {
        var response = await HttpClientService.ExecuteAsync<ProductCategoryResponseModel>(
            Endpoints.ProductCategory,
            EnumHttpMethod.Post,
            new ProductCategoryRequestModel
            {
                ProductCategoryCode = "001",
                ProductCategoryName = "Testing"
            }
        );
        if (response.IsError)
        {
            return;
        }

        MudDialog.Close(reqModel);
    }
}
