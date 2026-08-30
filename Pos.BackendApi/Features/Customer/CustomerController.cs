namespace Pos.BackendApi.Features.Customer;

[Route("api/v1/customers")]
[ApiController]
public class CustomerController : BaseController
{
    private readonly CustomerService _customer;
    private readonly ResponseModel _response;

    public CustomerController(
        IServiceProvider serviceProvider,
        CustomerService customer,
        ResponseModel response) : base(serviceProvider)
    {
        _customer = customer;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomer()
    {
        try
        {
            var customerLst = await _customer.GetCustomer();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = customerLst.DataLst.Count,
                IsSuccess = customerLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customerLst.MessageResponse.Message,
                Item = customerLst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetCustomer(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var customerLst = await _customer.GetCustomer(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = customerLst.DataLst.Count,
                IsSuccess = customerLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customerLst.MessageResponse.Message,
                Item = customerLst.DataLst,
                PageSetting = customerLst.PageSetting
            });

            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{customerCode}")]
    public async Task<IActionResult> GetCustomerByCode(string customerCode)
    {
        try
        {
            var customer = await _customer.GetCustomerByCode(customerCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.MessageResponse.Message,
                Item = customer.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerModel requestModel)
    {
        try
        {
            var customer = await _customer.CreateCustomer(requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
                Item = requestModel
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerModel requestModel)
    {
        try
        {
            var customer = await _customer.UpdateCustomer(id, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
                Item = requestModel
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var customer = await _customer.DeleteCustomer(id);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}