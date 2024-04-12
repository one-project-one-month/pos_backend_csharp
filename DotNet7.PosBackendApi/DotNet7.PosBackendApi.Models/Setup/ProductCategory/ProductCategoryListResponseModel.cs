namespace DotNet7.PosBackendApi.Models.Setup.ProductCategory;

public class ProductCategoryListResponseModel
{
    public List<ProductCategoryModel> DataList { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}
