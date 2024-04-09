namespace DotNet7.PosBackendApi.Features.Setup.Staff;

public class DL_Staff
{
    private readonly AppDbContext _context;

    public DL_Staff(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StaffModel>> GetStaffs()
    {
        var staffList = await _context
            .TblStaffs
            .AsNoTracking()
            .ToListAsync();
        return staffList.Select(x => x.Change()).ToList();
    }

    public async Task<StaffModel> GetStaff(int id)
    {
        var staff = await _context
            .TblStaffs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == id);
        var responseModel = staff is not null ? staff.Change() : new StaffModel();
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateStaff(StaffModel requestModel)
    {
        await _context.TblStaffs.AddAsync(requestModel.Change());
        var result = await _context.SaveChangesAsync();
        var responseModel = result > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateStaff(int id, StaffModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        var shop = await _context
            .TblStaffs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == id);
        if (shop == null)
        {
            responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
            goto result;
        }

        await _context.TblStaffs.AddAsync(requestModel.Change());
        var result = await _context.SaveChangesAsync();
        responseModel = result > 0
            ? new MessageResponseModel(false, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteStaff(int id)
    {
        var responseModel = new MessageResponseModel();
        var staff = await _context
            .TblStaffs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == id);
        if (staff is null)
        {
            responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
            goto result;
        }

        _context.Remove(staff);
        var result = await _context.SaveChangesAsync();
        responseModel = result > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        result:
        return responseModel;
    }
}