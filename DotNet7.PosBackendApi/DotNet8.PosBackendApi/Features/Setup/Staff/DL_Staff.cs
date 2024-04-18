using DotNet8.PosBackendApi.DbService.Models;
using DotNet8.PosBackendApi.Models;
using DotNet8.PosBackendApi.Models.Setup.Staff;
using Microsoft.EntityFrameworkCore;

namespace DotNet8.PosBackendApi.Features.Setup.Staff;

public class DL_Staff
{
    private readonly AppDbContext _context;

    public DL_Staff(AppDbContext context)
    {
        _context = context;
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

        return responseModel;
    }

    // private async Task<string> GenerateUserCode()
    // {
    //     string userCode = string.Empty;
    //     var user = await _context.TblStaffs
    //         .AsNoTracking()
    //         .OrderByDescending(x => x.StaffId)
    //         .FirstOrDefaultAsync();
    //     if (user is null)
    //     {
    //         userCode = "U00001";
    //         goto Result;
    //     }
    //
    //     var codeNo = user.UserCode.Split('U')[1].ToInt32();
    //     userCode = codeNo switch
    //     {
    //         < 10 => $"U0000{codeNo + 1}",
    //         < 100 => $"U000{codeNo + 1}",
    //         < 1000 => $"U00{codeNo + 1}",
    //         < 10000 => $"U0{codeNo + 1}",
    //         _ => $"U{codeNo + 1}" 
    //     };
    //
    //     Result:
    //     return userCode;
    // }

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