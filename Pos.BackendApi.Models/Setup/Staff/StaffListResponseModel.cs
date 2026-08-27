using DotNet8.PosBackendApi.Models.Setup.PageSetting;

namespace DotNet8.PosBackendApi.Models.Setup.Staff;

public class StaffListResponseModel
{
    public List<StaffModel> DataList { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}