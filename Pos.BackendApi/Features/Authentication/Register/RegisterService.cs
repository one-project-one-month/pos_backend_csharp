namespace Pos.BackendApi.Features.Authentication.Register;

public class RegisterService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerate _token;
    private readonly JwtModel _tokenModel;

    public RegisterService(IOptionsMonitor<JwtModel> tokenModel, AppDbContext context, JwtTokenGenerate token)
    {
        _context = context;
        _token = token;
        _tokenModel = tokenModel.CurrentValue;
    }

    public async Task<MessageResponseModel> CreateStaff(StaffModel requestModel)
    {
        CheckStaffNullValue(requestModel);

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
            _token.GenerateAccessToken(requestModel);
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

    private static void CheckStaffNullValue(StaffModel staff)
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
