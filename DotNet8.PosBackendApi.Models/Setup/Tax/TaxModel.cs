namespace DotNet8.PosBackendApi.Models.Setup.Tax;

public class TaxModel
{
    public int TaxId { get; set; }
    public int FromAmount { get; set; }
    public int ToAmount { get; set; }
    public decimal? Percentage { get; set; }
    public int? FixedAmount { get; set; }
}