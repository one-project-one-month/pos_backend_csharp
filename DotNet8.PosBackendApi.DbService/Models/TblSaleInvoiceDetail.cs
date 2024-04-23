using System;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.DbService.Models;

public partial class TblSaleInvoiceDetail
{
    public int SaleInvoiceDetailId { get; set; }

    public string VoucherNo { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Amount { get; set; }
}
