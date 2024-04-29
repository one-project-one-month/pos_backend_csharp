using Azure;
using DotNet8.PosBackendApi.DbService.Models;
using DotNet8.PosBackendApi.Models.Setup.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DotNet8.PosBackendApi.Features.Customer
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly BL_Customer _bL_Customer;
        private readonly ResponseModel _response;
        private readonly JwtTokenGenerate _token;

        public CustomerController(
            IServiceProvider serviceProvider, 
            BL_Customer bL_Customer, 
            ResponseModel response, 
            JwtTokenGenerate token) : base(serviceProvider)
        {
            _bL_Customer = bL_Customer;
            _response = response;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var customerLst = await _bL_Customer.GetCustomer();
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

        [HttpGet("{customerCode}")]
        public async Task<IActionResult> GetCustomerByCode(string customerCode)
        {
            try
            {
                var customer = await _bL_Customer.GetCustomerByCode(customerCode);
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
        public async Task<IActionResult> Create(CustomerModel requestModel)
        {
            try
            {
                var customer = await _bL_Customer.Create(requestModel);
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
            };
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, CustomerModel requestModel)
        {
            try
            {
                var customer = await _bL_Customer.Update(id, requestModel);
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
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customer = await _bL_Customer.Delete(id);
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
            };
        }
    }
}
