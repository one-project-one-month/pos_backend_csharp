using DotNet8.PosFrontendBlazor.Models.SaleInvoice;

namespace DotNet8.PosFrontendBlazor.Models.Township
{
    public class SaleInvoiceResponseModel : ResponseModel
    {
        public SaleInvoiceItemModel Data { get; set; } = new SaleInvoiceItemModel();
    }
}
