using Pos.BackendApi.Models.Setup.Token;

namespace Pos.BackendApi.Models.Setup.Product;

public class ProductResponseModel: TokenResponseModel
{
    public ProductModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}
