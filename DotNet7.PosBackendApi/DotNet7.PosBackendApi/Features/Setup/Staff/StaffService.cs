using DotNet7.PosBackendApi.DbService.DbModels;
using DotNet7.PosBackendApi.Models.Setup.Staff;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<StaffModel> GetStaff(int id)
        {
            // var lst = await _context.TblStaffs.FindAsync(id);
            var lst = await _context.TblStaffs.FirstOrDefaultAsync(x => x.StaffId == id);
            var StaffModel = new StaffModel
            {
                Address = lst.Address,
                DateOfBirth = lst.DateOfBirth,
                Gender = lst.Gender,
                MobileNo = lst.MobileNo,
                Position = lst.Position,
                StaffCode = lst.StaffCode,
                StaffId = lst.StaffId,
                StaffName = lst.StaffName
            };
            return StaffModel;
        }

        public async Task AddStaff(StaffModel staffmodel)
        {
            TblStaff model = new TblStaff()
            {
                Address = staffmodel.Address,
                DateOfBirth = staffmodel.DateOfBirth,
                Gender = staffmodel.Gender,
                MobileNo = staffmodel.MobileNo,
                Position = staffmodel.Position,
                StaffCode = staffmodel.StaffCode,
                StaffId = staffmodel.StaffId,
                StaffName = staffmodel.StaffName
            };
          
            await _context.TblStaffs.AddAsync(model);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStaff(StaffModel staffmodel)
        {
          
            TblStaff staff = await _context.TblStaffs.FindAsync(staffmodel.StaffId);

            staff.Address = staffmodel.Address;
            staff.DateOfBirth = staffmodel.DateOfBirth;
            staff.Gender = staffmodel.Gender;
            staff.MobileNo = staffmodel.MobileNo;
            staff.Position = staffmodel.Position;
            staff.StaffCode = staffmodel.StaffCode;
            staff.StaffName = staffmodel.StaffName;

            _context.TblStaffs.Update(staff);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteStaff(int id)
       {
            TblStaff staff = await _context.TblStaffs.FindAsync(id);
            if (staff != null)
            {
                _context.TblStaffs.Remove(staff);
                await _context.SaveChangesAsync();
            }

        }
    }
}
