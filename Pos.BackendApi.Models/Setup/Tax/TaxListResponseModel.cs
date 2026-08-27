namespace Pos.BackendApi.Models.Setup.Tax;

public class TaxListResponseModel
{
    public List<TaxModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
