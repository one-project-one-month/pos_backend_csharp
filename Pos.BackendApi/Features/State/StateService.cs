namespace Pos.BackendApi.Features.State;

public class StateService
{
    private readonly AppDbContext _context;

    public StateService(AppDbContext context) => _context = context;

    public async Task<StateListResponseModel> GetState()
    {
        var responseModel = new StateListResponseModel();
        try
        {
            var states = await _context
                .TblPlaceStates
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataLst = states
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<StateModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }

        return responseModel;
    }

    public async Task<StateListResponseModel> GetState(int pageNo, int pageSize)
    {
        var responseModel = new StateListResponseModel();
        try
        {
            var query = _context
                .TblPlaceStates
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
                pageCount++;
            var lst = await query.
                Pagination(pageNo, pageSize)
                .ToListAsync();

            responseModel.Data = new StateDataModel
            {
                State = lst.Select(x => x.Change()).ToList(),
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<StateModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }

        return responseModel;
    }

    public async Task<StateResponseModel> GetStateByCode(string StateCode)
    {
        if (StateCode is null) throw new Exception("StateCode is null");

        var responseModel = new StateResponseModel();
        try
        {
            var state = await _context
                .TblPlaceStates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StateCode == StateCode);
            if (state is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = state.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new StateModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateState(StateModel requestModel)
    {
        CheckStateNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var stateCode = await _context.TblPlaceStates.AsNoTracking().MaxAsync(x => x.StateCode);
            requestModel.StateCode = stateCode.GenerateStateCode(EnumCodePrefix.MMR.ToString(), 2);
            await _context.TblPlaceStates.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateState(int id, StateModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var state = await _context
                .TblPlaceStates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StateId == id);

            if (state is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.StateName))
                state.StateName = requestModel.StateName;

            _context.TblPlaceStates.Update(state);
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteState(int id)
    {
        if (id <= 0) throw new Exception("id is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var state = await _context
                .TblPlaceStates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StateId == id);
            if (state == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblPlaceStates.Remove(state);
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    private void CheckStateNullValue(StateModel State)
    {
        if (State is null)
            throw new Exception("State is null.");

        if (string.IsNullOrWhiteSpace(State.StateName))
            throw new Exception("StateName is null.");
    }
}
