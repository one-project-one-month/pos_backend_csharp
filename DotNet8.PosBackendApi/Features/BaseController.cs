using Microsoft.EntityFrameworkCore;

namespace DotNet8.PosBackendApi.Features;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
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

    protected string RefreshToken()
    {
        var token = Request.Headers
                .FirstOrDefault(x => x.Key == "Authorization")
                .Value
                .ToString()
                .Substring("Bearer ".Length) ?? throw new Exception("Invalid Token.");
        return token;
    }

    protected string RefreshToken(AppDbContext context, JwtTokenGenerate tokenGenerate)
    {
        string token = string.Empty;
        var model = context.TblStaffs.FirstOrDefault();
        if (model == null)
            token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAiLCJTdGFmZk5hbWUiOiJTdSBTdSIsIlN0YWZmQ29kZSI6IlUwMDAwMSIsIlRva2VuRXhwaXJlZCI6IjIwMjQtMDQtMjJUMTY6MzY6NDMuNjE1MTc1NFoiLCJuYmYiOjE3MTM4MDI5MDMsImV4cCI6MTcxMzgwMzgwMywiaWF0IjoxNzEzODAyOTAzfQ.IA6JMyYx1yaM2K9ch38sS1Fr2eukLKjOOhh-u5oPTI4";
        else
        {
            token = tokenGenerate.GenerateAccessToken(model.Change());
        }
        return token;
    }
}
