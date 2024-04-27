namespace DotNet8.PosBackendApi.Features;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public BaseController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected IActionResult InternalServerError(Exception ex)
    {
        return StatusCode(500, new
        {
            Message = ex.ToString()
        });
    }

    protected IActionResult Content(object obj)
    {
        return Content(JsonConvert.SerializeObject(obj), "application/json");
    }

    protected string RefreshTokenV2()
    {
        var token = Request.Headers
                .FirstOrDefault(x => x.Key == "Authorization")
                .Value
                .ToString()
                .Substring("Bearer ".Length) ?? throw new Exception("Invalid Token.");
        return token;
    }

    protected string RefreshToken()
    {
        var tokenGenerate = _serviceProvider.GetRequiredService<JwtTokenGenerate>();

        string token = string.Empty;

        #region No Token

        // if (!Request.Headers.Any(x => x.Key == "Authorization"))
        if (Request.Headers.All(x => x.Key != "Authorization"))
        {
            // Inject AppDbContext
            var context = _serviceProvider.GetRequiredService<AppDbContext>();

            // Get First Row From Staff
            var model = context.TblStaffs.FirstOrDefault();
            if (model == null)
            {
                // Default Token
                token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAiLCJTdGFmZk5hbWUiOiJTdSBTdSIsIlN0YWZmQ29kZSI6IlUwMDAwMSIsIlRva2VuRXhwaXJlZCI6IjIwMjQtMDQtMjJUMTY6MzY6NDMuNjE1MTc1NFoiLCJuYmYiOjE3MTM4MDI5MDMsImV4cCI6MTcxMzgwMzgwMywiaWF0IjoxNzEzODAyOTAzfQ.IA6JMyYx1yaM2K9ch38sS1Fr2eukLKjOOhh-u5oPTI4";
                goto result;
            }

            // Default Staff to Generate Token
            token = tokenGenerate.GenerateAccessToken(model.Change());
            goto result;
        }
        #endregion

        #region Exist Token

        token = Request.Headers
           .FirstOrDefault(x => x.Key == "Authorization")
           .Value
           .ToString()
           .Substring("Bearer ".Length) ?? throw new Exception("Invalid Token.");

        // Refresh Token or Regeneate Token
        token = tokenGenerate.GenerateRefreshToken(token);

    #endregion

    result:
        return token;
    }
}
