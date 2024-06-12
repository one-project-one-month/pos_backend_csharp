namespace DotNet8.PosFrontendBlazor.Models.SaleInvoice;

public class SaleInvoiceListResponseModel
{
    public SaleInvoiceDataModel Data { get; set; } = new SaleInvoiceDataModel();
    public PageSettingModel PageSetting { get; set; }
}