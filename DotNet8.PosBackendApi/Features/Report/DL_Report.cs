namespace DotNet8.PosBackendApi.Features.Report;

public class DL_Report
{
    private readonly AppDbContext _context;

    public DL_Report(AppDbContext context) => _context = context;

    public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
            responseModel.Data = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .Where(x => x.SaleInvoiceDateTime.Month == month && x.SaleInvoiceDateTime.Year == year)
                .GroupBy(x => x.SaleInvoiceDateTime.Date)
                .Select(y => new ReportModel
                {
                    SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                    TotalAmount = y.Sum(c => c.TotalAmount)
                }).OrderBy(x => x.SaleInvoiceDate).ToListAsync();
            responseModel.MessageResponse = responseModel.Data.Count > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(int year)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();

        responseModel.Data = await _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Year == year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate).ToListAsync();
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());

        return responseModel;
    }
}