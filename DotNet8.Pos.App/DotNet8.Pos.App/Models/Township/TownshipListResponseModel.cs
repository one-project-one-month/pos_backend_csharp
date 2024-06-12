namespace DotNet8.PosFrontendBlazor.Models.Township;

public class TownshipListResponseModel : ResponseModel
{
    public TownshipDataModel Data { get; set; } = new TownshipDataModel();
    public PageSettingModel PageSetting { get; set; }
}