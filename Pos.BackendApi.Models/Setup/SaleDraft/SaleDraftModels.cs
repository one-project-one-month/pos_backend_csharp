using System.ComponentModel.DataAnnotations;

namespace Pos.BackendApi.Models.Setup.SaleDraft;

public class SaleDraftSummaryModel
{
    public int SaleDraftId { get; set; }
    public string DraftName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class SaleDraftModel : SaleDraftSummaryModel
{
    public List<SaleDraftItemModel> Items { get; set; } = [];
}

public sealed class SaleDraftItemModel
{
    public int SaleDraftDetailId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount => Quantity * UnitPrice;
}

public sealed class CreateSaleDraftRequestModel
{
    [StringLength(100)]
    public string? DraftName { get; set; }
}

public sealed class AddSaleDraftItemRequestModel
{
    [Required, StringLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Range(1, 999)]
    public int Quantity { get; set; } = 1;

    [Required]
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class SetSaleDraftItemQuantityRequestModel
{
    [Range(0, 999)]
    public int Quantity { get; set; }

    [Required]
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class CheckoutSaleDraftRequestModel
{
    [StringLength(50)]
    public string? CustomerCode { get; set; }

    [Required, StringLength(20)]
    public string CustomerAccountNo { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string PaymentType { get; set; } = "Cash";

    [Range(0, double.MaxValue)]
    public decimal Discount { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal ReceiveAmount { get; set; }

    [Required]
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class SaleDraftCheckoutResponseModel
{
    public int SaleInvoiceId { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Tax { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal Change { get; set; }
}
