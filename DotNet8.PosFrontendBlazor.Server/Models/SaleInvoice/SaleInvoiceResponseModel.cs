namespace DotNet8.PosFrontendBlazor.Server.Models.SaleInvoice;

public class SaleInvoiceResponseModel : ResponseModel
{
    public SaleInvoiceItemModel Data { get; set; } = new SaleInvoiceItemModel();
}