namespace DotNet8.PosBackendApi.Models.Setup.Township;

public class TownshipListResponseModel
{
    public List<TownshipModel> DataList { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public TownshipDataModel Data { get; set; }
}

public class TownshipDataModel
{
    public PageSettingModel PageSetting { get; set; }
    public List<TownshipModel> Township { get; set; }
}
