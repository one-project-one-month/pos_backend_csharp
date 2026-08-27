using Pos.BackendApi.Models.Setup.PageSetting;

namespace Pos.BackendApi.Models.Setup.Staff;

public class StaffListResponseModel
{
    public List<StaffModel> DataList { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}