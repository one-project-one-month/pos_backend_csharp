using DotNet8.PosBackendApi.Models.Setup.Tax;

namespace DotNet8.PosBackendApi.Features.Tax;

public class DL_Tax
{
    private readonly AppDbContext _context;

    public DL_Tax(AppDbContext context)
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
                .ToListAsync();

            taxListResponseModel.DataLst = lst.Select(x => x.Change()).ToList();
            taxListResponseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            taxListResponseModel.DataLst = [];
            taxListResponseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        return taxListResponseModel;
    }

    public async Task<TaxResponseModel> GetTaxById(int id)
    {
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
            responseModel.Data = null;
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }
    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateTax(TaxModel requestModel)
    {
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

    }
}
