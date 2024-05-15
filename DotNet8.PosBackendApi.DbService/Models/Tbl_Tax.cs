namespace DotNet8.PosBackendApi.DbService.Models
{
    public class Tbl_Tax
    {
        public int TaxId { get; set; }
        public int FromAmount { get; set; }
        public int ToAmount { get; set; }
        public decimal? Percentage { get; set; }
        public int? FixedAmount { get; set; }
    }
}
