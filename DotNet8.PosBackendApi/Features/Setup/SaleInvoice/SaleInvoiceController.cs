using Microsoft.AspNetCore.Mvc;

namespace DotNet8.PosBackendApi.Features.Setup.SaleInvoice
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleInvoiceController : BaseController
    {
        private readonly BL_SaleInvoice _saleInvoice;
        private readonly ResponseModel _response;
        private readonly JwtTokenGenerate _token;

        public SaleInvoiceController(BL_SaleInvoice saleInvoice, ResponseModel response, JwtTokenGenerate token)
        {
            _saleInvoice = saleInvoice;
            _response = response;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetSaleInovice()
        {
            try
            {
                var lst = await _saleInvoice.GetSaleInvoice();
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = _token.GenerateRefreshToken(RefreshToken()),
                        Count = lst.DataList.Count,
                        EnumPos = EnumPos.SaleInvoice,
                        IsSuccess = lst.MessageResponse.IsSuccess,
                        Message = lst.MessageResponse.Message,
                        Item = lst.DataList
                    });
                return Content(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("{voucherNo}")]
        public async Task<IActionResult> GetSaleInovice(string voucherNo)
        {
            try
            {
                var lst = await _saleInvoice.GetSaleInvoice(voucherNo);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = _token.GenerateRefreshToken(RefreshToken()),
                        EnumPos = EnumPos.SaleInvoice,
                        IsSuccess = lst.MessageResponse.IsSuccess,
                        Message = lst.MessageResponse.Message,
                        Item = lst.Data
                    });
                return Content(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
