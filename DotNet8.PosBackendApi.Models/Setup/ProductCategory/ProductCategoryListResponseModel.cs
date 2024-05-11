namespace DotNet8.PosBackendApi.Models.Setup.ProductCategory;

public class ProductCategoryListResponseModel
{
    public List<ProductCategoryModel> DataList { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public ProductCategoryDataModel Data { get; set; }
}

public class ProductCategoryDataModel
{
    public PageSettingModel PageSetting { get; set; }
    public List<ProductCategoryModel> ProductCategory { get; set; }
}