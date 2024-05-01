namespace DotNet8.PosBackendApi.Features.SaleInvoice;

public class BL_SaleInvoice
{
    private readonly DL_SaleInvoice _saleInvoice;

    public BL_SaleInvoice(DL_SaleInvoice saleInvoice) => _saleInvoice = saleInvoice;

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(DateTime startDate, DateTime endDate)
    {
        var response = await _saleInvoice.GetSaleInvoice(startDate, endDate);
        return response;
    }

    public async Task<SaleInvoiceResponseModel> GetSaleInvoice(string voucherNo)
    {
        var response = await _saleInvoice.GetSaleInvoice(voucherNo);
        return response;
    }

    public async Task<SaleInvoiceResponseModel> CreateSaleInvoice(SaleInvoiceModel saleInvoice)
    {
        var response = await _saleInvoice.CreateSaleInvoice(saleInvoice);
        return response;
    }

    public async Task<MessageResponseModel> UpdateSaleInvoice(int id, SaleInvoiceModel saleInvoice)
    {
        if (id <= 0) throw new Exception("SaleInvoiceId is null");
        var response = await _saleInvoice.UpdateSaleInvoice(id, saleInvoice);
        return response;
    }

    public async Task<MessageResponseModel> DeleteSaleInvoice(int id)
    {
        if (id <= 0) throw new Exception("SaleInvoiceId is null");
        var response = await _saleInvoice.DeleteSaleInvoice(id);
        return response;
    }
}