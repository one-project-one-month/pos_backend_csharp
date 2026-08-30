namespace Pos.BackendApi.Features.Tax;

public class TaxService
{
    private readonly AppDbContext _context;

    public TaxService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaxListResponseModel> GetTaxList()
    {
        TaxListResponseModel taxListResponseModel = new();
        try
        {
            var lst = await _context.Tbl_Taxes
                .AsNoTracking()
                .OrderByDescending(x => x.TaxId)
                .ToListAsync();

            taxListResponseModel.DataLst = lst.Select(x => x.Change()).ToList();
            taxListResponseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            taxListResponseModel.DataLst = new List<TaxModel>();
            taxListResponseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        return taxListResponseModel;
    }

    public async Task<TaxListResponseModel> GetTaxList(int pageNo, int pageSize, string? search = null)
    {
        TaxListResponseModel taxListResponseModel = new();
        try
        {
            var query = _context
                .Tbl_Taxes
                .OrderByDescending(x => x.TaxId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FromAmount.ToString().Contains(search) ||
                    x.ToAmount.ToString().Contains(search) ||
                    x.Percentage.ToString().Contains(search));
            }

            var tax = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;

            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            taxListResponseModel.DataLst = tax.Select(x => x.Change()).ToList();
            taxListResponseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            taxListResponseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        }
        catch (Exception ex)
        {
            taxListResponseModel.DataLst = new List<TaxModel>();
            taxListResponseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        return taxListResponseModel;
    }

    public async Task<TaxResponseModel> GetTaxById(int id)
    {
        if (id == 0)
            throw new Exception("Id cannot be empty.");

        TaxResponseModel responseModel = new();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = item.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new TaxModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }
    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateTax(TaxModel requestModel)
    {
        CheckTaxModel(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            await _context.Tbl_Taxes.AddAsync(requestModel.Change());
            int result = await _context.SaveChangesAsync();
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

    public async Task<MessageResponseModel> UpdateTax(int id, TaxModel requestModel)
    {
        if (id <= 0)
            throw new Exception("Id cannot be empty.");

        if (requestModel.FromAmount <= 0)
            throw new Exception("From Amount cannot be empty.");

        if (requestModel.ToAmount <= 0)
            throw new Exception("To Amount cannot be empty.");

        if (requestModel.Percentage == 0 && requestModel.FixedAmount == 0)
            throw new Exception();

        if (string.IsNullOrEmpty(requestModel.TaxType))
            throw new Exception("Tax Type cannot be empty.");

        if (requestModel.Percentage > 0)
        {
            if (requestModel.Percentage <= 0 || requestModel.Percentage >= 100)
                throw new Exception("Percentage is invalid.");
        }

        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (requestModel.FromAmount != 0)
                item.FromAmount = requestModel.FromAmount;

            if (requestModel.ToAmount != 0)
                item.ToAmount = requestModel.ToAmount;

            if (requestModel.Percentage != 0)
            {
                item.Percentage = requestModel.Percentage;
                item.FixedAmount = default;
            }

            if (requestModel.FixedAmount != 0)
            {
                item.FixedAmount = requestModel.FixedAmount;
                item.Percentage = default;
            }

            if (!string.IsNullOrEmpty(requestModel.TaxType))
                item.TaxType = requestModel.TaxType;

            _context.Entry(item).State = EntityState.Modified;
            int result = await _context.SaveChangesAsync();
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

    public async Task<MessageResponseModel> DeleteTax(int id)
    {
        if (id <= 0)
            throw new Exception("Id cannot be empty.");

        MessageResponseModel responseModel = new();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, nameof(EnumStatus.NotFound));
                return responseModel;
            }

            _context.Tbl_Taxes.Remove(item);
            int result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, nameof(EnumStatus.Success))
                : new MessageResponseModel(false, nameof(EnumStatus.Fail));

            return responseModel;
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex.Message);
        }
        return responseModel;
    }

    private static void CheckTaxModel(TaxModel requestModel)
    {
        if (requestModel.FromAmount <= 0)
            throw new Exception("From Amount cannot be empty.");

        if (requestModel.ToAmount <= 0)
            throw new Exception("To Amount cannot be empty.");

        if (requestModel.Percentage == 0 && requestModel.FixedAmount == 0)
            throw new Exception();

        if (string.IsNullOrEmpty(requestModel.TaxType))
            throw new Exception("Tax Type cannot be empty.");

        if (requestModel.Percentage > 0)
        {
            if (requestModel.Percentage <= 0 || requestModel.Percentage >= 100)
                throw new Exception("Percentage is invalid.");
        }
    }
}
