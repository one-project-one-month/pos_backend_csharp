using DotNet8.PosBackendApi.Models.Setup.Token;

namespace DotNet8.PosBackendApi.Models.Setup.Product;

public class ProductListResponseModel:TokenResponseModel
{
    public List<ProductModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }

}
