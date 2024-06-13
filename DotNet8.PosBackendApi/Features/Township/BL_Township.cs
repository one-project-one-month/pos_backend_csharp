namespace DotNet8.PosBackendApi.Features.Township;

public class BL_Township
{
    private readonly DL_Township _dL_Township;

    public BL_Township(DL_Township dL_Township) => _dL_Township = dL_Township;

    public async Task<TownshipListResponseModel> GetTownship()
    {
        var response = await _dL_Township.GetTownship();
        return response;
    }

    public async Task<TownshipListResponseModel> GetTownship(int pageNo, int pageSize)
    {
        var response = await _dL_Township.GetTownship(pageNo, pageSize);
        return response;
    }

    public async Task<TownshipResponseModel> GetTownshipByCode(string TownshipCode)
    {
        if (TownshipCode is null) throw new Exception("TownshipCode is null");
        var response = await _dL_Township.GetTownshipByCode(TownshipCode);
        return response;
    }

    public async Task<TownshipListResponseModel> GetTownshipByStateCode(string stateCode)
    {
        if (stateCode is null) throw new Exception("StateCode is null");
        var response = await _dL_Township.GetTownshipByStateCode(stateCode);
        return response;
    }

    public async Task<MessageResponseModel> CreateTownship(TownshipModel requestModel)
    {
        CheckTownshipNullValue(requestModel);
        var response = await _dL_Township.CreateTownship(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateTownship(int id, TownshipModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");
        // CheckProductNullValue(requestModel);
        var response = await _dL_Township.UpdateTownship(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteTownship(int id)
    {
        if (id <= 0) throw new Exception("id is null");
        var response = await _dL_Township.DeleteTownship(id);
        return response;
    }

    private void CheckTownshipNullValue(TownshipModel Township)
    {
        if (Township is null)
        {
            throw new Exception("Township is null.");
        }

        //if (string.IsNullOrWhiteSpace(Township.TownshipCode))
        //{
        //    throw new Exception("Township.TownshipCode is null.");
        //}

        if (string.IsNullOrWhiteSpace(Township.TownshipName))
            throw new Exception("Township.TownshipName is null.");

        if (string.IsNullOrWhiteSpace(Township.StateCode))
            throw new Exception("Township.StateCode is null.");
    }
}