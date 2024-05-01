namespace DotNet8.PosBackendApi.Features.Customer;

public class BL_Customer
{
    private readonly DL_Customer _dL_Customer;

    public BL_Customer(DL_Customer dL_Customer) => _dL_Customer = dL_Customer;

    public async Task<CustomerListResponseModel> GetCustomer()
    {
        var response = await _dL_Customer.GetCustomer();
        return response;
    }

    public async Task<CustomerResponseModel> GetCustomerByCode(string customerCode)
    {
        if (customerCode is null) throw new Exception("customerCode is null");
        var response = await _dL_Customer.GetCustomerByCode(customerCode);
        return response;
    }

    public async Task<MessageResponseModel> CreateCustomer(CustomerModel requestModel)
    {
        CheckProductNullValue(requestModel);
        var response = await _dL_Customer.CreateCustomer(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateCustomer(int id, CustomerModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");
        // CheckProductNullValue(requestModel);
        var response = await _dL_Customer.UpdateCustomer(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteCustomer(int id)
    {
        if (id <= 0) throw new Exception("id is null");
        var response = await _dL_Customer.DeleteCustomer(id);
        return response;
    }

    private void CheckProductNullValue(CustomerModel customer)
    {
        if (customer is null)
        {
            throw new Exception("customer is null.");
        }

        if (string.IsNullOrWhiteSpace(customer.Gender))
        {
            throw new Exception("customer.Gender is null.");
        }

        if (string.IsNullOrWhiteSpace(customer.CustomerName))
        {
            throw new Exception("customer.CustomerName is null.");
        }

        if (customer.DateOfBirth == default)
        {
            throw new Exception("customer.DateOfBirth is null.");
        }

        var age = CalculateAge(customer.DateOfBirth);

        if (age >= 40 || age <= 18)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 18 and 40 (exclusive).");
        }

        if (string.IsNullOrWhiteSpace(customer.StateCode))
        {
            throw new Exception("customer.StateCode is null.");
        }

        if (string.IsNullOrWhiteSpace(customer.TownshipCode))
        {
            throw new Exception("customer.TownshipCode is null.");
        }

        if (string.IsNullOrWhiteSpace(customer.MobileNo))
        {
            throw new Exception("customer.MobileNo is null.");
        }
    }

    private int CalculateAge(DateTime birthdate)
    {
        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - birthdate;
        int age = (int)(ageDifference.TotalDays / 365); // Approximate years
        return age;
    }
}