using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet7.PosBackendApi.Features.Setup.Staff
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StaffController : BaseController
    {
        private readonly StaffService _staffService;

        public StaffController(StaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                return Ok(await _staffService.GetStaffs());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
