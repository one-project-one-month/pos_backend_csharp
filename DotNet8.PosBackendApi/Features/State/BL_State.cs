namespace DotNet8.PosBackendApi.Features.State;

public class BL_State
{
    private readonly DL_State _dL_State;

    public BL_State(DL_State dL_State) => _dL_State = dL_State;

    public async Task<StateListResponseModel> GetState()
    {
        var response = await _dL_State.GetState();
        return response;
    }
    //public async Task<TownshipListResponseModel> GetTownship(int pageNo, int pageSize)
    //{
    //    var response = await _dL_Township.GetTownship(pageNo, pageSize);
    //    return response;
    //}
    public async Task<StateListResponseModel> GetState(int pageNo, int pageSize)
    {
        var response = await _dL_State.GetState(pageNo, pageSize);
        return response;
    }
    public async Task<StateResponseModel> GetStateByCode(string StateCode)
    {
        if (StateCode is null) throw new Exception("StateCode is null");
        var response = await _dL_State.GetStateByCode(StateCode);
        return response;
    }

    public async Task<MessageResponseModel> CreateState(StateModel requestModel)
    {
        CheckStateNullValue(requestModel);
        var response = await _dL_State.CreateState(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateState(int id, StateModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");
        var response = await _dL_State.UpdateState(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteState(int id)
    {
        if (id <= 0) throw new Exception("id is null");
        var response = await _dL_State.DeleteState(id);
        return response;
    }

    private void CheckStateNullValue(StateModel State)
    {
        if (State is null)
            throw new Exception("State is null.");

        if (string.IsNullOrWhiteSpace(State.StateName))
            throw new Exception("StateName is null.");

    }
}