using DotNet7.PosBackendApi.DbService.DbModels;
using DotNet7.PosBackendApi.Models.Setup.Staff;
using Microsoft.EntityFrameworkCore;

namespace DotNet7.PosBackendApi.Features.Setup.Staff
{
    public class StaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StaffModel>> GetStaffs()
        {
            var lst = await _context.TblStaffs.ToListAsync();
            return lst.Select(x => new StaffModel
            {
                Address = x.Address,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                MobileNo = x.MobileNo,
                Position = x.Position,
                StaffCode = x.StaffCode,
                StaffId = x.StaffId,
                StaffName = x.StaffName,
            }).ToList();
        }
    }
}
