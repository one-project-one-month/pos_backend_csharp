using DotNet8.PosBackendApi.Models.Setup.Product;

namespace DotNet8.PosBackendApi.Models.Setup.Township;

public class TownshipListResponseModel
{
    public List<TownshipModel> DataLst { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}
