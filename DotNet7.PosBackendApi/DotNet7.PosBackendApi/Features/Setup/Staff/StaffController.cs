using DotNet7.PosBackendApi.Models.Setup.Staff;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(int id)
        {
            try
            {
                var staff = await _staffService.GetStaff(id);
                if (staff == null)
                {
                    return NotFound();
                }
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffModel staffmodel)
        {
            try
            {
                if (staffmodel == null)
                {
                    return BadRequest("Staff is null");
                }
                await _staffService.AddStaff(staffmodel);
                return CreatedAtAction(nameof(GetStaff), new { id = staffmodel.StaffId }, staffmodel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StaffModel staff)
        {

            var existingUser = await _staffService.GetStaff(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.StaffName = staff.StaffName;
            existingUser.StaffCode = staff.StaffCode;
            existingUser.DateOfBirth = staff.DateOfBirth;
            existingUser.Address = staff.Address;
            existingUser.Gender = staff.Gender;
            existingUser.Position = staff.Position;
            existingUser.MobileNo = staff.MobileNo;

            await _staffService.UpdateStaff(existingUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var staff = await _staffService.GetStaff(id);
                if (staff == null)
                {
                    return NotFound();
                }
                await _staffService.DeleteStaff(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
