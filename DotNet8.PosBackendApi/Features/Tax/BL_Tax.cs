using DotNet8.PosBackendApi.Models.Setup.Tax;

namespace DotNet8.PosBackendApi.Features.Tax;

public class BL_Tax
{
    private readonly DL_Tax _dL_Tax;

    public BL_Tax(DL_Tax dL_Tax)
    {
        _dL_Tax = dL_Tax;
    }

    public async Task<TaxListResponseModel> GetTaxList()
    {
        return await _dL_Tax.GetTaxList();
    }
}
