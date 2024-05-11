namespace DotNet8.PosBackendApi.Models.Setup.Customer;

public class CustomerResponseModel : TokenResponseModel
{
    public CustomerModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}