namespace DotNet8.PosFrontendBlazor.Server.Models.ProductCategory;

public class ProductCategoryModel
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryCode { get; set; } = null!;
    public string ProductCategoryName { get; set; } = null!;
}