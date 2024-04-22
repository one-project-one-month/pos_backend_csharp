namespace DotNet8.PosBackendApi.Models.Setup.Config;

public class OpenApiSecuritySchemeModel
{
    public OpenApiReferenceModel Reference { get; set; }
}

public class OpenApiReferenceModel
{
    public ReferenceType Type { get; set; }
    public string Id { get; set; }
}
