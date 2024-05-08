namespace DotNet8.PosFrontendBlazor.Models.Customer;

public class CustomerParamsModel
{
    public CustomerParamsModel(int customerID, string customerName, string mobileNo, DateTime? dateOfBirth, string gender, string statusCode, string townshipCode)
    {
        CustomerId = customerID;
        CustomerName = customerName;
        MobileNo = mobileNo;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        StateCode = statusCode;
        TownshipCode = townshipCode;
    }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string MobileNo { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string StateCode { get; set; }

    public string TownshipCode { get; set; }
}
