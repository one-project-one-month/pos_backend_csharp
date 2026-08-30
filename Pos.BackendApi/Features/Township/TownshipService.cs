namespace Pos.BackendApi.Features.Township;

public class TownshipService
{
    private readonly AppDbContext _context;

    public TownshipService(AppDbContext context) => _context = context;

    public async Task<TownshipListResponseModel> GetTownship()
    {
        var responseModel = new TownshipListResponseModel();
        try
        {
            var townships = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataList = townships
                .Select(x => x.Change())
                .OrderByDescending(x => x.TownshipId)
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<TownshipModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<TownshipListResponseModel> GetTownship(int pageNo, int pageSize)
    {
        var responseModel = new TownshipListResponseModel();
        try
        {
            var query = _context
                .TblPlaceTownships
                .AsNoTracking();
            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
                pageCount++;

            var lst = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            responseModel.Data = new TownshipDataModel
            {
                Township = lst.Select(x => x.Change()).ToList(),
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<TownshipModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<TownshipResponseModel> GetTownshipByCode(string TownshipCode)
    {
        if (TownshipCode is null) throw new Exception("TownshipCode is null");

        var responseModel = new TownshipResponseModel();
        try
        {
            var township = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TownshipCode == TownshipCode);
            if (township is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = township.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new TownshipModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    public async Task<TownshipListResponseModel> GetTownshipByStateCode(string stateCode)
    {
        if (stateCode is null) throw new Exception("StateCode is null");

        var responseModel = new TownshipListResponseModel();
        try
        {
            var townships = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .Where(x => x.StateCode == stateCode)
                .ToListAsync();
            responseModel.DataList = townships
                .Select(x => x.Change())
                .OrderByDescending(x => x.TownshipId)
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<TownshipModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> CreateTownship(TownshipModel requestModel)
    {
        CheckTownshipNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var townshipCode = await _context.TblPlaceTownships
                .AsNoTracking()
                .MaxAsync(x => x.TownshipCode);
            requestModel.TownshipCode = townshipCode.GenerateTownshipCode(EnumCodePrefix.MMR.ToString(), 2);
            await _context.TblPlaceTownships.AddAsync(requestModel.Change());
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

    public async Task<MessageResponseModel> UpdateTownship(int id, TownshipModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var township = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TownshipId == id);

            if (township is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.TownshipName))
                township.TownshipName = requestModel.TownshipName;

            if (!string.IsNullOrEmpty(requestModel.TownshipCode))
                township.TownshipCode = requestModel.TownshipCode;

            _context.TblPlaceTownships.Update(township);
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

    public async Task<MessageResponseModel> DeleteTownship(int id)
    {
        if (id <= 0) throw new Exception("id is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var township = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TownshipId == id);
            if (township == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblPlaceTownships.Remove(township);
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

    private void CheckTownshipNullValue(TownshipModel Township)
    {
        if (Township is null)
            throw new Exception("Township is null.");

        if (string.IsNullOrWhiteSpace(Township.TownshipName))
            throw new Exception("Township.TownshipName is null.");

        if (string.IsNullOrWhiteSpace(Township.StateCode))
            throw new Exception("Township.StateCode is null.");
    }
}
