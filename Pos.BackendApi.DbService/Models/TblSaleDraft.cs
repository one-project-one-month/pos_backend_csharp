namespace Pos.BackendApi.DbService.Models;

public sealed class TblSaleDraft
{
    public int SaleDraftId { get; set; }
    public int StaffId { get; set; }
    public string? DraftName { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public List<TblSaleDraftDetail> Details { get; set; } = [];
}
