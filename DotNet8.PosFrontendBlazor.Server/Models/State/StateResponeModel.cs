namespace DotNet8.PosFrontendBlazor.Server.Models.State;

public class StateResponeModel:ResponseModel
{
    public StateItemModel Data { get; set; }
}

public class StateItemModel
{
    public StateModel State { get; set; }
}