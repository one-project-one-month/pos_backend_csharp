namespace Pos.BackendApi.DbService.Models;

public sealed class TblSaleDraftDetail
{
    public int SaleDraftDetailId { get; set; }
    public int SaleDraftId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public TblSaleDraft SaleDraft { get; set; } = null!;
}
