using DotNet8.PosBackendApi.Models.Setup.ProductCategory;
using DotNet8.PosFrontendBlazor.Server.Models.Product;

namespace DotNet8.PosFrontendBlazor.Server.Components.Pages.ProductCategory;

public partial class P_ProductCategoryDeleteDialog
{

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string contentText { get; set; }
    [Parameter] public int productCategoryId { get; set; }

    private ProductRequestModel reqModel = new();
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task DeleteProductCategory(int id)
    {
        var reqModel = await HttpClientService.ExecuteAsync<ProductCategoryListResponseModel>(Endpoints.ProductCategory, EnumHttpMethod.Get);
        if (reqModel is not null)
        {
            await HttpClientService.ExecuteAsync<TownshipResponseModel>(
                Endpoints.ProductCategory + $"/{id}",
                EnumHttpMethod.Delete
            );
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            InjectService.ShowMessage("No data found.", EnumResponseType.Warning);
        }
    }
}