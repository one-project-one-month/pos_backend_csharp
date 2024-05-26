using DotNet8.PosFrontendBlazor.Models.Product;
using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using Newtonsoft.Json;
namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice;

public partial class P_SaleReceipt
{
    private EnumSaleInvoiceFormType saleInvoiceFormType = EnumSaleInvoiceFormType.Receipt;

    protected override void OnInitialized()
    {
        var uri = new Uri(Nav.Uri);
        Console.WriteLine(uri.Query);
    }
    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            
            //StateHasChanged();
            await InjectService.DisableLoading();
        }
    }
}

