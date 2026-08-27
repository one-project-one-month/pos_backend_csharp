namespace Pos.BackendApi.DbService.Models;

public sealed class TblRefreshToken
{
    public long RefreshTokenId { get; set; }
    public int StaffId { get; set; }
    public Guid FamilyId { get; set; }
    public byte[] TokenHash { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? UsedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public byte[]? ReplacedByTokenHash { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
