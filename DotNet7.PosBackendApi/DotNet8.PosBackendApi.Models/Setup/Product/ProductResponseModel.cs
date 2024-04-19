using DotNet8.PosBackendApi.Models.Setup.Token;

namespace DotNet8.PosBackendApi.Models.Setup.Product;

public class ProductResponseModel: TokenResponseModel
{
    public ProductModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}
