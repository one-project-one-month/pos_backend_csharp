namespace Pos.BackendApi.Features.Generate;

[Route("api/[controller]")]
[ApiController]
public class GenerateController : BaseController
{
    private readonly GenerateService _generate;
    private readonly ResponseModel _response;

    public GenerateController(IServiceProvider serviceProvider, GenerateService generate, ResponseModel response)
        : base(serviceProvider)
    {
        _generate = generate;
        _response = response;
    }

    [Route("sale-invoice/{year}")]
    [HttpPost]
    public async Task<IActionResult> Execute(int year)
    {
        var responseModel = await _generate.GenerateSaleInvoices(year);

        var model = _response.Return(new ReturnModel
        {
            Token = RefreshToken(),
            EnumPos = EnumPos.SaleInvoice,
            IsSuccess = responseModel.MessageResponse.IsSuccess,
            Message = responseModel.MessageResponse.Message
        });
        return Content(model);
    }

    [Route("product-categories")]
    [HttpPost]
    public async Task<IActionResult> ImportProductCategory()
    {
        var model = await _generate.ImportProductCategories();
        model.Token = RefreshToken();
        return Content(model);
    }

    [Route("products")]
    [HttpPost]
    public async Task<IActionResult> ImportProduct()
    {
        var model = await _generate.ImportProducts();
        model.Token = RefreshToken();
        return Content(model);
    }
}
