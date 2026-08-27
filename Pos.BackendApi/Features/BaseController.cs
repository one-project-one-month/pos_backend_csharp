namespace Pos.BackendApi.Features;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    public BaseController(IServiceProvider serviceProvider) { }

    protected IActionResult InternalServerError(Exception ex)
    {
        return Problem(
            title: "The request could not be completed.",
            detail: "An unexpected server error occurred.",
            statusCode: StatusCodes.Status500InternalServerError);
    }

    protected IActionResult Content(object obj)
    {
        return Content(JsonConvert.SerializeObject(obj), "application/json");
    }

    // Kept for the legacy response envelope. Token renewal is handled only by
    // POST /api/v1/auth/refresh and is never embedded in business responses.
    protected static string RefreshToken() => string.Empty;
}
