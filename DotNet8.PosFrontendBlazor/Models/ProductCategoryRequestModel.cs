namespace DotNet8.PosFrontendBlazor.Models
{
    public class ProductCategoryRequestModel
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategoryCode { get; set; } = null!;
        public string ProductCategoryName { get; set; } = null!;
    }
}
