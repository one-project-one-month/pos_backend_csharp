using System.Data;
using System.Security.Cryptography;

namespace Pos.BackendApi.Features.SaleDraft;

public sealed class SaleDraftService
{
    private readonly AppDbContext _context;

    public SaleDraftService(AppDbContext context) => _context = context;

    public async Task<List<SaleDraftSummaryModel>> ListAsync(int staffId)
    {
        return await _context.TblSaleDrafts
            .AsNoTracking()
            .Where(x => x.StaffId == staffId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .Select(x => new SaleDraftSummaryModel
            {
                SaleDraftId = x.SaleDraftId,
                DraftName = x.DraftName ?? $"Draft #{x.SaleDraftId}",
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc,
                ItemCount = x.Details.Sum(d => d.Quantity),
                TotalAmount = x.Details.Sum(d => d.Quantity * d.UnitPrice),
                RowVersion = Convert.ToBase64String(x.RowVersion),
            })
            .ToListAsync();
    }

    public async Task<SaleDraftModel> CreateAsync(int staffId, CreateSaleDraftRequestModel request)
    {
        var now = DateTime.UtcNow;
        var draft = new TblSaleDraft
        {
            StaffId = staffId,
            DraftName = string.IsNullOrWhiteSpace(request.DraftName) ? null : request.DraftName.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        };

        await _context.TblSaleDrafts.AddAsync(draft);
        await _context.SaveChangesAsync();
        return await GetRequiredAsync(draft.SaleDraftId, staffId);
    }

    public async Task<SaleDraftModel?> GetAsync(int draftId, int staffId)
    {
        var draft = await _context.TblSaleDrafts
            .AsNoTracking()
            .Include(x => x.Details)
            .FirstOrDefaultAsync(x => x.SaleDraftId == draftId && x.StaffId == staffId);
        return draft is null ? null : await ToModelAsync(draft);
    }

    public async Task<SaleDraftModel> AddItemAsync(
        int draftId,
        int staffId,
        AddSaleDraftItemRequestModel request)
    {
        var draft = await LoadOwnedDraftAsync(draftId, staffId);
        EnsureRowVersion(draft.RowVersion, request.RowVersion);

        var product = await _context.TblProducts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProductCode == request.ProductCode);
        if (product is null)
            throw new KeyNotFoundException("Product was not found.");

        var detail = draft.Details.FirstOrDefault(x => x.ProductCode == request.ProductCode);
        if (detail is null)
        {
            draft.Details.Add(new TblSaleDraftDetail
            {
                ProductCode = product.ProductCode,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
            });
        }
        else
        {
            detail.Quantity = checked(detail.Quantity + request.Quantity);
        }

        draft.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return await GetRequiredAsync(draftId, staffId);
    }

    public async Task<SaleDraftModel> SetQuantityAsync(
        int draftId,
        int staffId,
        string productCode,
        SetSaleDraftItemQuantityRequestModel request)
    {
        var draft = await LoadOwnedDraftAsync(draftId, staffId);
        EnsureRowVersion(draft.RowVersion, request.RowVersion);
        var detail = draft.Details.FirstOrDefault(x => x.ProductCode == productCode)
            ?? throw new KeyNotFoundException("Draft item was not found.");

        if (request.Quantity == 0)
            _context.TblSaleDraftDetails.Remove(detail);
        else
            detail.Quantity = request.Quantity;

        draft.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return await GetRequiredAsync(draftId, staffId);
    }

    public async Task<SaleDraftModel> RemoveItemAsync(
        int draftId,
        int staffId,
        string productCode,
        string? rowVersion)
    {
        var draft = await LoadOwnedDraftAsync(draftId, staffId);
        EnsureRowVersion(draft.RowVersion, rowVersion);
        var detail = draft.Details.FirstOrDefault(x => x.ProductCode == productCode)
            ?? throw new KeyNotFoundException("Draft item was not found.");
        _context.TblSaleDraftDetails.Remove(detail);
        draft.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return await GetRequiredAsync(draftId, staffId);
    }

    public async Task DeleteAsync(int draftId, int staffId, string? rowVersion)
    {
        var draft = await LoadOwnedDraftAsync(draftId, staffId);
        EnsureRowVersion(draft.RowVersion, rowVersion);
        _context.TblSaleDrafts.Remove(draft);
        await _context.SaveChangesAsync();
    }

    public async Task<SaleDraftCheckoutResponseModel> CheckoutAsync(
        int draftId,
        int staffId,
        CheckoutSaleDraftRequestModel request)
    {
        await using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.Serializable);

        var draft = await LoadOwnedDraftAsync(draftId, staffId);
        EnsureRowVersion(draft.RowVersion, request.RowVersion);
        if (draft.Details.Count == 0)
            throw new InvalidOperationException("Add at least one item before checkout.");

        var staff = await _context.TblStaffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == staffId)
            ?? throw new KeyNotFoundException("Staff account was not found.");

        var subtotal = draft.Details.Sum(x => x.Quantity * x.UnitPrice);
        var tax = await CalculateTaxAsync(subtotal);
        var paymentAmount = subtotal - request.Discount + tax;
        if (paymentAmount <= 0)
            throw new InvalidOperationException("Payment amount must be greater than zero.");
        if (request.ReceiveAmount < paymentAmount)
            throw new InvalidOperationException("Receive amount must cover the payment amount.");

        var voucherNo = await GenerateVoucherNoAsync();
        if (string.IsNullOrWhiteSpace(voucherNo))
            throw new InvalidOperationException("A voucher number could not be generated.");

        var invoice = new TblSaleInvoice
        {
            SaleInvoiceDateTime = DateTime.Now,
            VoucherNo = voucherNo,
            TotalAmount = subtotal,
            Discount = request.Discount,
            StaffCode = staff.StaffCode,
            Tax = tax,
            PaymentType = request.PaymentType,
            CustomerAccountNo = request.CustomerAccountNo,
            PaymentAmount = paymentAmount,
            ReceiveAmount = request.ReceiveAmount,
            Change = request.ReceiveAmount - paymentAmount,
            CustomerCode = string.IsNullOrWhiteSpace(request.CustomerCode) ? "C_001" : request.CustomerCode,
        };

        await _context.TblSaleInvoices.AddAsync(invoice);
        await _context.TblSaleInvoiceDetails.AddRangeAsync(draft.Details.Select(x => new TblSaleInvoiceDetail
        {
            VoucherNo = voucherNo,
            ProductCode = x.ProductCode,
            Quantity = x.Quantity,
            Price = x.UnitPrice,
            Amount = x.Quantity * x.UnitPrice,
        }));
        _context.TblSaleDrafts.Remove(draft);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new SaleDraftCheckoutResponseModel
        {
            SaleInvoiceId = invoice.SaleInvoiceId,
            VoucherNo = voucherNo,
            TotalAmount = subtotal,
            Tax = tax,
            PaymentAmount = paymentAmount,
            Change = invoice.Change ?? 0,
        };
    }

    private async Task<TblSaleDraft> LoadOwnedDraftAsync(int draftId, int staffId)
    {
        return await _context.TblSaleDrafts
            .Include(x => x.Details)
            .FirstOrDefaultAsync(x => x.SaleDraftId == draftId && x.StaffId == staffId)
            ?? throw new KeyNotFoundException("Sale draft was not found.");
    }

    private async Task<SaleDraftModel> GetRequiredAsync(int draftId, int staffId)
        => await GetAsync(draftId, staffId)
            ?? throw new KeyNotFoundException("Sale draft was not found.");

    private async Task<SaleDraftModel> ToModelAsync(TblSaleDraft draft)
    {
        var productCodes = draft.Details.Select(x => x.ProductCode).ToArray();
        var productNames = await _context.TblProducts.AsNoTracking()
            .Where(x => productCodes.Contains(x.ProductCode))
            .ToDictionaryAsync(x => x.ProductCode, x => x.ProductName);

        return new SaleDraftModel
        {
            SaleDraftId = draft.SaleDraftId,
            DraftName = draft.DraftName ?? $"Draft #{draft.SaleDraftId}",
            CreatedAtUtc = draft.CreatedAtUtc,
            UpdatedAtUtc = draft.UpdatedAtUtc,
            ItemCount = draft.Details.Sum(x => x.Quantity),
            TotalAmount = draft.Details.Sum(x => x.Quantity * x.UnitPrice),
            RowVersion = Convert.ToBase64String(draft.RowVersion),
            Items = draft.Details.OrderBy(x => x.SaleDraftDetailId).Select(x => new SaleDraftItemModel
            {
                SaleDraftDetailId = x.SaleDraftDetailId,
                ProductCode = x.ProductCode,
                ProductName = productNames.GetValueOrDefault(x.ProductCode, x.ProductCode),
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList(),
        };
    }

    private static void EnsureRowVersion(byte[] current, string? supplied)
    {
        if (string.IsNullOrWhiteSpace(supplied))
            throw new DBConcurrencyException("Draft version is required. Reload and try again.");

        byte[] expected;
        try
        {
            expected = Convert.FromBase64String(supplied);
        }
        catch (FormatException)
        {
            throw new DBConcurrencyException("Draft version is invalid.");
        }

        if (!CryptographicOperations.FixedTimeEquals(current, expected))
            throw new DBConcurrencyException("This draft changed in another request. Reload and try again.");
    }

    private async Task<string?> GenerateVoucherNoAsync()
    {
        var transaction = _context.Database.CurrentTransaction
            ?? throw new InvalidOperationException("Voucher generation requires an active transaction.");
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.Sp_GenerateSaleInvoiceNo";
        command.CommandType = CommandType.StoredProcedure;
        command.Transaction = transaction.GetDbTransaction();
        var value = await command.ExecuteScalarAsync();
        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    private async Task<decimal> CalculateTaxAsync(decimal amount)
    {
        var rules = await _context.Tbl_Taxes.AsNoTracking()
            .Where(x => amount >= x.FromAmount)
            .OrderBy(x => x.FromAmount)
            .ToListAsync();

        decimal tax = 0;
        foreach (var rule in rules)
        {
            var taxable = Math.Min(amount, rule.ToAmount) - rule.FromAmount;
            if (taxable < 0)
                continue;
            tax += taxable * ((rule.Percentage ?? 0) / 100) + (rule.FixedAmount ?? 0);
        }
        return tax;
    }
}
