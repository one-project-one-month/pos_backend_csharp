using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice;

public partial class P_SaleInvoiceDetailDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public string voucherNo { get; set; }

    private List<SaleInvoiceDetailModel> lstSaleInvoiceDetail { get; set; } = new List<SaleInvoiceDetailModel>();

    private int count = 0;

    protected override async void OnInitialized()
    {
        var lstDetail = await HttpClientService.ExecuteAsync<SaleInvoiceResponseModel>
        (
            $"{Endpoints.SaleInvoice}/{voucherNo}",
            EnumHttpMethod.Get
        );
        if (lstDetail != null)
        {
            lstSaleInvoiceDetail = lstDetail.Data.SaleInvoice.SaleInvoiceDetails;
            StateHasChanged();
        }
    }

    private void Cancel()
    {
        MudDialog?.Cancel();
    }
}