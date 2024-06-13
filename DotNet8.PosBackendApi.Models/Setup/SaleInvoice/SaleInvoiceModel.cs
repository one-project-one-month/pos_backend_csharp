using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.SaleInvoice;

public class SaleInvoiceModel
{
    public int SaleInvoiceId { get; set; }
    public DateTime SaleInvoiceDateTime { get; set; }
    public string? VoucherNo { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public string StaffCode { get; set; }
    public decimal Tax {  get; set; }
    public string? PaymentType { get; set; }
    public string? CustomerAccountNo { get; set; }
    public decimal? PaymentAmount { get; set; }
    public decimal? ReceiveAmount { get; set; }
    public decimal? Change { get; set; }
    public string? CustomerCode { get; set; }
    public List<SaleInvoiceDetailModel>? SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();
}