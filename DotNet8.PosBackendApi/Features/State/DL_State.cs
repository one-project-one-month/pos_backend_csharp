using DotNet8.PosBackendApi.Models.Setup.State;

namespace DotNet8.PosBackendApi.Features.State;

public class DL_State
{
    private readonly AppDbContext _context;

    public DL_State(AppDbContext context) => _context = context;

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
    public async Task<StateResponseModel> GetStateByCode(string stateCode)
    {
        var responseModel = new StateResponseModel();
        try
        {
            var state = await _context
                .TblPlaceStates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StateCode == stateCode);
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

            #region Patch Method Validation Conditions

            if (!string.IsNullOrEmpty(requestModel.StateName))
                state.StateName = requestModel.StateName;

            #endregion

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
}