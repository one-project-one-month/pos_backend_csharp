namespace DotNet8.PosFrontendBlazor.Models;

public class ProductCategoryListResponseModel : ResponseModel
{
    public ProductCategoryDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
