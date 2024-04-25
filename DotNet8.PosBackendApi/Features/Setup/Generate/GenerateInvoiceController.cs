using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.PosBackendApi.Features.Setup.Generate
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateInvoiceController : ControllerBase
    {
        //[HttpPost("{year}")]
        //public async Task<IActionResult> Execute(int year)
        //{

        //}

        // year
        // 1
        // random 20 - 50 (1 day) - 20 * 365
        // 1000, 20000 , 340000 - amount
        // 5 - 10 - detail products
    }
}
