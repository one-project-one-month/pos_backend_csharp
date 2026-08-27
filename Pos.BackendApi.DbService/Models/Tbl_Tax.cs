namespace DotNet8.PosBackendApi.DbService.Models;

public class Tbl_Tax
{
    public int TaxId { get; set; }
    public int FromAmount { get; set; }
    public int ToAmount { get; set; }
    public string TaxType { get; set; } = null!;
    public decimal? Percentage { get; set; }
    public decimal? FixedAmount { get; set; }
}