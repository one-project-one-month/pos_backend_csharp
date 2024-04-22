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
}
