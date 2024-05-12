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
}
