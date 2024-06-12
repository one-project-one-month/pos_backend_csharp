namespace DotNet8.PosFrontendBlazor.Models.SaleInvoice;

public class SaleInvoiceResponseModel : ResponseModel
{
    public SaleInvoiceItemModel Data { get; set; } = new SaleInvoiceItemModel();
}