namespace DotNet8.PosFrontendBlazor.Server.Models.State;

public class StateModel
{
    public int StateId { get; set; }

    public string StateCode { get; set; } = null!;

    public string StateName { get; set; } = null!;
}