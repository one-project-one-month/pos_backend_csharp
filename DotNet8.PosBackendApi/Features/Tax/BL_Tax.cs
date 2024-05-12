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

    public async Task<TaxResponseModel> GetTaxById(int id)
    {
        if (id == 0)
            throw new Exception("Id cannot be empty.");

        return await _dL_Tax.GetTaxById(id);
    }

    public async Task<MessageResponseModel> CreateTax(TaxModel requestModel)
    {
        if (requestModel.FromAmount <= 0)
            throw new Exception("From Amount cannot be empty.");

        if (requestModel.ToAmount <= 0)
            throw new Exception("To Amount cannot be empty.");

        if (requestModel.Percentage <= 0)
            throw new Exception("Percentage cannot be empty.");

        if (requestModel.Percentage >= 100)
            throw new Exception("Invalid Percentage");

        MessageResponseModel responseModel = await _dL_Tax.CreateTax(requestModel);

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

        if (requestModel.Percentage <= 0)
            throw new Exception("Percentage cannot be empty.");

        if (requestModel.Percentage >= 100)
            throw new Exception("Invalid Percentage");

        MessageResponseModel responseModel = await _dL_Tax.UpdateTax(id, requestModel);
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteTax(int id)
    {
        if (id <= 0)
            throw new Exception("Id cannot be empty.");

        MessageResponseModel responseModel = await _dL_Tax.DeleteTax(id);
        return responseModel;
    }
}
