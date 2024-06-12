namespace DotNet8.PosFrontendBlazor.Models.ProductCategory;

public class ProductCategoryRequestModel
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryCode { get; set; } = null!;
    public string ProductCategoryName { get; set; } = null!;
}