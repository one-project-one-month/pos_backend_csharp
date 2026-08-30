using Microsoft.Extensions.Options;
using Pos.BackendApi.Models.Setup.PageSetting;

namespace Pos.BackendApi.Features.Staff;

public class StaffService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerate _token;
    private readonly JwtModel _tokenModel;

    public StaffService(IOptionsMonitor<JwtModel> tokenModel, AppDbContext context, JwtTokenGenerate token)
    {
        _context = context;
        _token = token;
        _tokenModel = tokenModel.CurrentValue;
    }

    public async Task<StaffListResponseModel> GetStaffs()
    {
        var responseModel = new StaffListResponseModel();
        try
        {
            var staffList = await _context
                .TblStaffs
                .AsNoTracking()
                .ToListAsync();

            responseModel.DataList = staffList.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<StaffModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<StaffListResponseModel> GetStaffs(int PageSize, int PageNo, string? search = null)
    {
        if (PageSize <= 0) throw new Exception("PageSize is not less than 0.");
        if (PageNo <= 0) throw new Exception("PageNo is not less than 0.");

        var responseModel = new StaffListResponseModel();
        try
        {
            var staffList = _context.TblStaffs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                staffList = staffList.Where(x =>
                    x.StaffCode.Contains(search) ||
                    x.StaffName.Contains(search) ||
                    x.MobileNo.Contains(search));
            }

            var staff = await staffList
                .Pagination(PageNo, PageSize)
                .ToListAsync();

            var totalCount = await staffList.CountAsync();
            var pageCount = totalCount / PageSize;
            if (totalCount % PageSize > 0)
                pageCount++;

            responseModel.DataList = staff.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            responseModel.PageSetting = new PageSettingModel(PageNo, PageSize, pageCount, totalCount);
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<StaffModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<StaffResponseModel> GetStaff(int id)
    {
        var responseModel = new StaffResponseModel();
        try
        {
            var staff = await _context
                .TblStaffs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StaffId == id);
            responseModel.Data = staff is not null ? staff.Change() : new StaffModel();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> CreateStaff(StaffModel requestModel)
    {
        CheckShopNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var staffCode = await _context.TblStaffs
                .AsNoTracking()
                .MaxAsync(x => x.StaffCode);
            requestModel.StaffCode = staffCode.GenerateCode(EnumCodePrefix.S.ToString());
            requestModel.Password = requestModel.Password.ToHash(_tokenModel.Key);
            await _context.TblStaffs.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            var token = _token.GenerateAccessToken(requestModel);
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<StaffResponseModel> GetStaffByMobileNo(string MobileNo)
    {
        if (MobileNo is null) throw new Exception("MobileNo is null");

        var responseModel = new StaffResponseModel();
        try
        {
            var Staff = await _context
                .TblStaffs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MobileNo == MobileNo);
            if (Staff is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = Staff.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new StaffModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateStaff(int id, StaffModel requestModel)
    {
        if (id == 0) throw new Exception("id is 0.");
        CheckShopNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var staff = await _context.TblStaffs.FirstOrDefaultAsync(x => x.StaffId == id);

            if (staff is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.StaffCode))
                staff.StaffCode = requestModel.StaffCode;

            if (!string.IsNullOrEmpty(requestModel.StaffName))
                staff.StaffName = requestModel.StaffName;

            if (!string.IsNullOrEmpty(requestModel.Position))
                staff.Position = requestModel.Position;

            if (!string.IsNullOrEmpty(requestModel.Password))
                staff.Password = requestModel.Password;

            if (!string.IsNullOrEmpty(requestModel.Address))
                staff.Address = requestModel.Address;

            if (!string.IsNullOrEmpty(requestModel.MobileNo))
                staff.MobileNo = requestModel.MobileNo;

            if (requestModel.DateOfBirth != null)
                staff.DateOfBirth = requestModel.DateOfBirth;

            if (!string.IsNullOrEmpty(requestModel.Gender))
                staff.Gender = requestModel.Gender;

            _context.Entry(staff).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteStaff(int id)
    {
        if (id == 0) throw new Exception("id is 0.");

        var responseModel = new MessageResponseModel();
        try
        {
            var staff = await _context
                .TblStaffs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StaffId == id);
            if (staff is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblStaffs.Remove(staff);
            _context.Entry(staff).State = EntityState.Deleted;
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    private void CheckShopNullValue(StaffModel staff)
    {
        if (staff is null)
            throw new Exception("Staff is null.");

        if (string.IsNullOrWhiteSpace(staff.StaffCode))
            throw new Exception("StaffCode is null.");

        if (string.IsNullOrWhiteSpace(staff.StaffName))
            throw new Exception("StaffName is null.");

        if (string.IsNullOrWhiteSpace(staff.MobileNo))
            throw new Exception("Staff MobileNo is null.");

        if (string.IsNullOrWhiteSpace(staff.Address))
            throw new Exception("Staff Address is null.");

        if (string.IsNullOrWhiteSpace(staff.DateOfBirth.ToString()))
            throw new Exception("Staff DateOfBirth is null.");

        if (string.IsNullOrWhiteSpace(staff.Gender))
            throw new Exception("Staff Gender is null.");

        if (string.IsNullOrWhiteSpace(staff.Position))
            throw new Exception("Staff Position is null.");
    }
}
