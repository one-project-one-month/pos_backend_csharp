namespace DotNet8.PosFrontendBlazor.Server.Models.State;

public class StateListResponseModel:ResponseModel
{
    public StateDataModel Data { get; set; } = new StateDataModel();
    public PageSettingModel PageSetting { get; set; }
}