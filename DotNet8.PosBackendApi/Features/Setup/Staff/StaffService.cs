namespace DotNet8.PosBackendApi.Features.Setup.Staff;

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
        var item = await _context.TblStaffs.FirstOrDefaultAsync(x => x.StaffId == id);
        if (item is null) throw new Exception("Invalid Staff.");

        var StaffModel = new StaffModel
        {
            Address = item.Address,
            DateOfBirth = item.DateOfBirth,
            Gender = item.Gender,
            MobileNo = item.MobileNo,
            Position = item.Position,
            StaffCode = item.StaffCode,
            StaffId = item.StaffId,
            StaffName = item.StaffName
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
        var item = await _context.TblStaffs.FindAsync(staffmodel.StaffId);
        if (item is null) throw new Exception("Invalid Staff.");

        item.Address = staffmodel.Address;
        item.DateOfBirth = staffmodel.DateOfBirth;
        item.Gender = staffmodel.Gender;
        item.MobileNo = staffmodel.MobileNo;
        item.Position = staffmodel.Position;
        item.StaffCode = staffmodel.StaffCode;
        item.StaffName = staffmodel.StaffName;

        _context.TblStaffs.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStaff(int id)
    {
        var staff = await _context.TblStaffs.FindAsync(id);
        if (staff is not null)
        {
            _context.TblStaffs.Remove(staff);
            await _context.SaveChangesAsync();
        }
    }
}