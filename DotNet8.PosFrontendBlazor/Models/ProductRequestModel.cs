namespace DotNet8.PosFrontendBlazor.Models
{
    public class ProductRequestModel
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public double ProductPrice { get; set; }
    }
}
