namespace DotNet8.PosBackendApi.Features.SaleInvoice
{
    [Route("api/v1/sale-invoices")]
    [ApiController]
    public class SaleInvoiceController : BaseController
    {
        private readonly BL_SaleInvoice _saleInvoice;
        private readonly ResponseModel _response;
        private readonly JwtTokenGenerate _token;
        private readonly AppDbContext _context;

        public SaleInvoiceController(IServiceProvider serviceProvider, BL_SaleInvoice saleInvoice, ResponseModel response, JwtTokenGenerate token, AppDbContext context) : base(serviceProvider)
        {
            _saleInvoice = saleInvoice;
            _response = response;
            _token = token;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSaleInovice(DateTime startDate, DateTime endDate)
        {
            try
            {
                var lst = await _saleInvoice.GetSaleInvoice(startDate, endDate);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = RefreshToken(),
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
                        Token = RefreshToken(),
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

        [HttpPost]
        public async Task<IActionResult> CreateSaleInvoice(SaleInvoiceModel requestModel)
        {
            try
            {
                var responseModel = await _saleInvoice.CreateSaleInvoice(requestModel);
                var model = _response.Return(
                    new ReturnModel
                    {
                        Token = RefreshToken(),
                        EnumPos = EnumPos.SaleInvoice,
                        IsSuccess = responseModel.MessageResponse.IsSuccess,
                        Message = responseModel.MessageResponse.Message,
                        Item = responseModel.Data
                    });
                return Content(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSaleInovice(int id, SaleInvoiceModel requestModel)
        {
            try
            {
                var item = await _saleInvoice.UpdateSaleInvoice(id, requestModel);
                var model = _response.Return(new ReturnModel
                {
                    Token = RefreshToken(),
                    EnumPos = EnumPos.SaleInvoice,
                    IsSuccess = item.IsSuccess,
                    Message = item.Message,
                    Item = item
                });
                return Content(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleInovice(int id)
        {
            try
            {
                var item = await _saleInvoice.DeleteSaleInvoice(id);
                var model = _response.Return(new ReturnModel
                {
                    Token = RefreshToken(),
                    EnumPos = EnumPos.SaleInvoice,
                    IsSuccess = item.IsSuccess,
                    Message = item.Message,
                    Item = item
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
