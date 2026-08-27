namespace Pos.BackendApi.Models.Setup.Customer;

public class CustomerResponseModel : TokenResponseModel
{
    public CustomerModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}