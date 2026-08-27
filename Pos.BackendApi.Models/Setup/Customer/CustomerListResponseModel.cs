using Pos.BackendApi.Models.Setup.PageSetting;

namespace Pos.BackendApi.Models.Setup.Customer;

public class CustomerListResponseModel
{
    public List<CustomerModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}