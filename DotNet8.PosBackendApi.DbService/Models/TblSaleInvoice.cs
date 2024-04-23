using System;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.DbService.Models;

public partial class TblSaleInvoice
{
    public int SaleInvoiceId { get; set; }

    public DateTime SaleInvoiceDateTime { get; set; }

    public string VoucherNo { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal Discount { get; set; }

    public string StaffCode { get; set; } = null!;

    public decimal Tax { get; set; }

    public string? PaymentType { get; set; }

    public string? CustomerAccountNo { get; set; }

    public decimal? PaymentAmount { get; set; }

    public decimal? ReceiveAmount { get; set; }

    public decimal? Change { get; set; }

    public string? CustomerCode { get; set; }
}
