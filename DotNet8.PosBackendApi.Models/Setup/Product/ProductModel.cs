namespace DotNet8.PosBackendApi.Models.Setup.Product;

public class ProductModel
{
    public int ProductId { get; set; }

    public string? ProductCode { get; set; } = null!;

    public string? ProductCategoryCode { get; set; } = null!;

    public string? ProductName { get; set; } = null!;

    public decimal Price { get; set; }
}


