namespace DotNet8.PosFrontendBlazor.Server.Models.Report;

public class ReportModel
{
    public DateTime? SaleInvoiceDate { get; set; }
    public decimal TotalAmount { get; set; } = 0;
}