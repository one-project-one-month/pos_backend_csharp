namespace DotNet8.PosFrontendBlazor.Server.Models.Shop;

public class ShopListResponseModel : ResponseModel
{
    public ShopDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}