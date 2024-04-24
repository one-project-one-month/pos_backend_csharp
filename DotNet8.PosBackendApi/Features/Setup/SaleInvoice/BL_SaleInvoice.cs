using DotNet8.PosBackendApi.Models.Setup.SaleInvoice;

namespace DotNet8.PosBackendApi.Features.Setup.SaleInvoice
{
    public class BL_SaleInvoice
    {
        private readonly DL_SaleInvoice _saleInvoice;

        public BL_SaleInvoice(DL_SaleInvoice saleInvoice)
        {
            _saleInvoice = saleInvoice;
        }

        public async Task<SaleInvoiceListResponseModel> GetSaleInvoice()
        {
            var response = await _saleInvoice.GetSaleInvoice();
            return response;
        }

        public async Task<SaleInvoiceResponseModel> GetSaleInvoice(string voucherNo)
        {
            var response = await _saleInvoice.GetSaleInvoice(voucherNo);
            return response;
        }

        public async Task<MessageResponseModel> CreateSaleInvoice(SaleInvoiceModel saleInvoice)
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

    }
}
