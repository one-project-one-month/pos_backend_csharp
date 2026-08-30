using Pos.BackendApi.Models.Setup.PageSetting;

namespace Pos.BackendApi.Features.Customer;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context) => _context = context;

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

    public async Task<CustomerListResponseModel> GetCustomer(int pageNo, int pageSize, string? search = null)
    {
        if (pageNo == 0)
            throw new Exception("Page No cannot be empty.");

        if (pageSize == 0)
            throw new Exception("Page Size cannot be empty.");

        var responseModel = new CustomerListResponseModel();
        try
        {
            var query = _context
                .TblCustomers
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.CustomerCode.Contains(search) ||
                    x.CustomerName.Contains(search) ||
                    x.MobileNo.Contains(search));
            }

            var customers = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
                pageCount++;

            responseModel.DataLst = customers
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
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
        if (customerCode is null) throw new Exception("customerCode is null");

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

    public async Task<MessageResponseModel> CreateCustomer(CustomerModel requestModel)
    {
        CheckCustomerNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var customerCode = await _context.TblCustomers
                .AsNoTracking()
                .MaxAsync(x => x.CustomerCode);
            requestModel.CustomerCode = customerCode.GenerateCode(EnumCodePrefix.C.ToString());
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

    public async Task<MessageResponseModel> UpdateCustomer(int id, CustomerModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var customer = await _context.TblCustomers.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == id);

            if (customer is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.CustomerCode))
                customer.CustomerCode = requestModel.CustomerCode;

            if (!string.IsNullOrEmpty(requestModel.CustomerName))
                customer.CustomerName = requestModel.CustomerName;

            if (!string.IsNullOrEmpty(requestModel.Gender))
                customer.Gender = requestModel.Gender;

            if (!string.IsNullOrEmpty(requestModel.MobileNo))
                customer.MobileNo = requestModel.MobileNo;

            if (requestModel.DateOfBirth != null)
                customer.DateOfBirth = requestModel.DateOfBirth;

            if (!string.IsNullOrEmpty(requestModel.StateCode))
                customer.StateCode = requestModel.StateCode;

            if (!string.IsNullOrEmpty(requestModel.TownshipCode))
                customer.TownshipCode = requestModel.TownshipCode;

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

    public async Task<MessageResponseModel> DeleteCustomer(int id)
    {
        if (id <= 0) throw new Exception("id is null");

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

    private void CheckCustomerNullValue(CustomerModel customer)
    {
        if (customer is null)
            throw new Exception("customer is null.");

        if (string.IsNullOrEmpty(customer.CustomerName))
            throw new Exception("CustomerName is null");

        if (string.IsNullOrWhiteSpace(customer.Gender))
            throw new Exception("customer Gender is null.");

        if (string.IsNullOrWhiteSpace(customer.CustomerName))
            throw new Exception("customer CustomerName is null.");

        if (customer.DateOfBirth == default)
            throw new Exception("customer DateOfBirth is null.");

        var age = CalculateAge(customer.DateOfBirth);

        if (age >= 40 || age <= 18)
            throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 18 and 40 (exclusive).");

        if (string.IsNullOrWhiteSpace(customer.StateCode))
            throw new Exception("customer StateCode is null.");

        if (string.IsNullOrWhiteSpace(customer.TownshipCode))
            throw new Exception("customer TownshipCode is null.");

        if (string.IsNullOrWhiteSpace(customer.MobileNo))
            throw new Exception("customer MobileNo is null.");
    }

    private int CalculateAge(DateTime birthdate)
    {
        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - birthdate;
        int age = (int)(ageDifference.TotalDays / 365);
        return age;
    }
}
