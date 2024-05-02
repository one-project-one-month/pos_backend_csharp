namespace DotNet8.PosBackendApi.Models.Setup.Township;

public class TownshipResponseModel : TokenResponseModel
{
    public TownshipModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}