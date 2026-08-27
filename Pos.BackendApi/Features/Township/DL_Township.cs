namespace DotNet8.PosBackendApi.Features.Township;

public class DL_Township
{
    private readonly AppDbContext _context;

    public DL_Township(AppDbContext context) => _context = context;

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

    public async Task<TownshipResponseModel> GetTownshipByCode(string townshipCode)
    {
        var responseModel = new TownshipResponseModel();
        try
        {
            var township = await _context
                .TblPlaceTownships
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TownshipCode == townshipCode);
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

    private async Task<string> GenerateTownshipCode()
    {
        string customerCode = string.Empty;
        if (!await _context.TblPlaceTownships.AnyAsync())
        {
            customerCode = "MMR001";
            goto result;
        }

        var maxStaffCode = await _context.TblPlaceTownships
            .AsNoTracking()
            .MaxAsync(x => x.TownshipCode);

        maxStaffCode = maxStaffCode.Substring(3);
        int staffCode = Convert.ToInt32(maxStaffCode) + 1;
        customerCode = $"MMR{staffCode.ToString().PadLeft(2, '0')}";
    result:
        return customerCode;
    }

    public async Task<MessageResponseModel> UpdateTownship(int id, TownshipModel requestModel)
    {
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

            #region Patch Method Validation Conditions

            if (!string.IsNullOrEmpty(requestModel.TownshipName))
                township.TownshipName = requestModel.TownshipName;

            if (!string.IsNullOrEmpty(requestModel.TownshipCode))
                township.TownshipCode = requestModel.TownshipCode;

            #endregion

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
}