using Effortless.Net.Encryption;
using Microsoft.Extensions.Options;

namespace DotNet8.PosBackendApi.Features.Setup.Staff;

public class DL_Staff
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerate _token;
    private readonly JwtModel _tokenModel;

    public DL_Staff(IOptionsMonitor<JwtModel> tokenModel, AppDbContext context, JwtTokenGenerate token)
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
        var responseModel = new MessageResponseModel();
        try
        {
            requestModel.StaffCode = await GenerateUserCode();
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

    private async Task<string> GenerateUserCode()
    {
        string userCode = string.Empty;
        if (!await _context.TblStaffs.AnyAsync())
        {
            userCode = "U00001";
            goto result;
        }

        var maxStaffCode = await _context.TblStaffs
            .AsNoTracking()
            .MaxAsync(x => x.StaffCode);

        maxStaffCode = maxStaffCode.Substring(1);
        int staffCode = Convert.ToInt32(maxStaffCode) + 1;
        userCode = $"U{staffCode.ToString().PadLeft(5, '0')}";
        result:
        return userCode;
    }

    public async Task<MessageResponseModel> UpdateStaff(int id, StaffModel requestModel)
    {
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

            await _context.TblStaffs.AddAsync(requestModel.Change());
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

    public async Task<MessageResponseModel> DeleteStaff(int id)
    {
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

            _context.Remove(staff);
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
}