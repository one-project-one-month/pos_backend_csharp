using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet7.PosBackendApi.Features
{
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
    }
}
