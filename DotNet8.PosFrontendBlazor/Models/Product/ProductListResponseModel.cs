namespace DotNet8.PosFrontendBlazor.Models.Product;

public class ProductListResponseModel : ResponseModel
{
    public ProductDataModel Data { get; set; }

    public PageSettingModel PageSetting { get; set; }
}