namespace DotNet8.PosBackendApi.Models.Setup.Customer
{
    public class CustomerListResponseModel
    {
        public List<CustomerModel> DataLst { get; set; }
        public MessageResponseModel MessageResponse { get; set; }
    }
}