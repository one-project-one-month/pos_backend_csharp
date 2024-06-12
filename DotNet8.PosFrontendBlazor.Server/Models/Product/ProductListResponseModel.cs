namespace DotNet8.PosFrontendBlazor.Server.Models.Product;

public class ProductListResponseModel : ResponseModel
{
    public ProductDataModel Data { get; set; }

    public PageSettingModel PageSetting { get; set; }
}