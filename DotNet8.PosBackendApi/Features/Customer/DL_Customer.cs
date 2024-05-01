namespace DotNet8.PosBackendApi.Features.Customer;

public class DL_Customer
{
    private readonly AppDbContext _context;

    public DL_Customer(AppDbContext context) => _context = context;

    public async Task<CustomerListResponseModel> GetCustomer()
    {
        var responseModel = new CustomerListResponseModel();
        try
        {
            var customers = await _context
                .TblCustomers
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataLst = customers
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<CustomerModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<CustomerResponseModel> GetCustomerByCode(string customerCode)
    {
        var responseModel = new CustomerResponseModel();
        try
        {
            var customer = await _context
                .TblCustomers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CustomerCode == customerCode);
            if (customer is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = customer.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new CustomerModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> Create(CustomerModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            requestModel.CustomerCode = await GenerateUserCode();
            await _context.TblCustomers.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    private async Task<string> GenerateUserCode()
    {
        string customerCode = string.Empty;
        if (!await _context.TblCustomers.AnyAsync())
        {
            customerCode = "C00001";
            goto result;
        }

        var maxStaffCode = await _context.TblCustomers
            .AsNoTracking()
            .MaxAsync(x => x.CustomerCode);

        maxStaffCode = maxStaffCode.Substring(1);
        int staffCode = Convert.ToInt32(maxStaffCode) + 1;
        customerCode = $"C{staffCode.ToString().PadLeft(5, '0')}";
        result:
        return customerCode;
    }

    public async Task<MessageResponseModel> Update(int id, CustomerModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var customer = await _context.TblCustomers.FirstOrDefaultAsync(x => x.CustomerId == id);

            if (customer is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.CustomerCode))
            {
                customer.CustomerCode = requestModel.CustomerCode;
            }

            if (!string.IsNullOrEmpty(requestModel.CustomerName))
            {
                customer.CustomerName = requestModel.CustomerName;
            }

            if (!string.IsNullOrEmpty(requestModel.Gender))
            {
                customer.Gender = requestModel.Gender;
            }

            if (!string.IsNullOrEmpty(requestModel.MobileNo))
            {
                customer.MobileNo = requestModel.MobileNo;
            }

            if (requestModel.DateOfBirth != null)
            {
                customer.DateOfBirth = requestModel.DateOfBirth;
            }

            if (!string.IsNullOrEmpty(requestModel.StateCode))
            {
                customer.StateCode = requestModel.StateCode;
            }

            if (!string.IsNullOrEmpty(requestModel.TownshipCode))
            {
                customer.TownshipCode = requestModel.TownshipCode;
            }

            _context.TblCustomers.Update(customer);
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> Delete(int id)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var customer = await _context
                .TblCustomers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CustomerId == id);
            if (customer == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblCustomers.Remove(customer);
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }
}