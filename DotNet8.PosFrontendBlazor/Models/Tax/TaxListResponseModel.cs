using DotNet8.PosBackendApi.Models;

namespace DotNet8.PosFrontendBlazor.Models.Tax;

public class TaxListResponseModel
{
    public List<TaxModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
