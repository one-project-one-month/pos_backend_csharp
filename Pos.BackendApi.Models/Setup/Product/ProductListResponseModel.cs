using Pos.BackendApi.Models.Setup.Token;

namespace Pos.BackendApi.Models.Setup.Product;

public class ProductListResponseModel:TokenResponseModel
{
    public List<ProductModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }

}
