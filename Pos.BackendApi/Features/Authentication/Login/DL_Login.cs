namespace Pos.BackendApi.Features.Authentication.Login;

public class DL_Login
{
    private readonly JwtTokenGenerate _tokenGenerator;
    private readonly AppDbContext _context;
    private readonly JwtModel _tokenModel;

    public DL_Login(JwtTokenGenerate tokenGenerator,
        AppDbContext context, IOptionsMonitor<JwtModel> tokenModel)
    {
        _tokenGenerator = tokenGenerator;
        _context = context;
        _tokenModel = tokenModel.CurrentValue;
    }

    public async Task<LoginResponseModel> Login(LoginRequestModel reqModel)
    {
        var item = await _context.TblStaffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffName == reqModel.UserName);

        if (item is null || item.Password != reqModel.Password.ToHash(_tokenModel.Key))
        {
            return new LoginResponseModel
            {
                Message = new MessageResponseModel(false, "Username or password is incorrect."),
            };
        }

        return await IssueTokenPairAsync(item, Guid.NewGuid(), DateTime.UtcNow.AddDays(_tokenModel.RefreshTokenDays));
    }

    public async Task<LoginResponseModel> Refresh(string rawRefreshToken)
    {
        var hash = JwtTokenGenerate.HashRefreshToken(rawRefreshToken);
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var current = await _context.TblRefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash.SequenceEqual(hash));

        if (current is null)
            return Failure("Refresh token is invalid.");

        if (current.RevokedAtUtc.HasValue || current.UsedAtUtc.HasValue)
        {
            await RevokeFamilyAsync(current.FamilyId);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Failure("Refresh token reuse was detected. Please sign in again.");
        }

        if (current.ExpiresAtUtc <= DateTime.UtcNow)
            return Failure("Refresh token has expired.");

        var staff = await _context.TblStaffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == current.StaffId);
        if (staff is null)
            return Failure("The account no longer exists.");

        current.UsedAtUtc = DateTime.UtcNow;
        current.RevokedAtUtc = DateTime.UtcNow;

        var response = await IssueTokenPairAsync(staff, current.FamilyId, current.ExpiresAtUtc, saveChanges: false);
        current.ReplacedByTokenHash = JwtTokenGenerate.HashRefreshToken(response.RefreshToken);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return response;
    }

    public async Task Revoke(string rawRefreshToken)
    {
        var hash = JwtTokenGenerate.HashRefreshToken(rawRefreshToken);
        var current = await _context.TblRefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash.SequenceEqual(hash));
        if (current is null)
            return;

        await RevokeFamilyAsync(current.FamilyId);
        await _context.SaveChangesAsync();
    }

    private async Task<LoginResponseModel> IssueTokenPairAsync(
        TblStaff staff,
        Guid familyId,
        DateTime refreshExpiresAtUtc,
        bool saveChanges = true)
    {
        var staffModel = staff.Change();
        var access = _tokenGenerator.GenerateAccessTokenWithExpiry(staffModel);
        var rawRefreshToken = JwtTokenGenerate.GenerateRefreshToken();
        await _context.TblRefreshTokens.AddAsync(new TblRefreshToken
        {
            StaffId = staff.StaffId,
            FamilyId = familyId,
            TokenHash = JwtTokenGenerate.HashRefreshToken(rawRefreshToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = refreshExpiresAtUtc,
        });

        if (saveChanges)
            await _context.SaveChangesAsync();

        return new LoginResponseModel
        {
            AccessToken = access.Token,
            AccessTokenExpiresAtUtc = access.ExpiresAtUtc,
            RefreshToken = rawRefreshToken,
            RefreshTokenExpiresAtUtc = refreshExpiresAtUtc,
            Staff = new AuthenticatedStaffModel
            {
                StaffId = staff.StaffId,
                StaffCode = staff.StaffCode,
                StaffName = staff.StaffName,
                Position = staff.Position,
            },
            Message = new MessageResponseModel(true, "Login successful."),
        };
    }

    private async Task RevokeFamilyAsync(Guid familyId)
    {
        var activeTokens = await _context.TblRefreshTokens
            .Where(x => x.FamilyId == familyId && x.RevokedAtUtc == null)
            .ToListAsync();
        foreach (var token in activeTokens)
            token.RevokedAtUtc = DateTime.UtcNow;
    }

    private static LoginResponseModel Failure(string message) => new()
    {
        Message = new MessageResponseModel(false, message),
    };
}
