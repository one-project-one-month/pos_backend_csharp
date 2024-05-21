using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNet8.PosBackendApi.Features.Report;

public class DL_Report
{
    private readonly AppDbContext _context;

    public DL_Report(AppDbContext context) => _context = context;

    public async Task<MonthlyReportResponseModel> DailyReport(int DateDay, int DateMonth, int DateYear, int PageNo, int PageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Day >= DateDay && x.SaleInvoiceDateTime.Month >= DateMonth && x.SaleInvoiceDateTime.Year == DateYear)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int TotalCount = query.Count();
        int PageCount = TotalCount / PageSize;
        if (TotalCount % PageSize != 0)
        {
            PageCount = PageCount + 1;
        }
        var report = await query
                .Pagination(PageNo, PageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(PageNo, PageSize, PageCount, TotalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }
    public async Task<MonthlyReportResponseModel> DailyReportV1(SaleDailyReportRequestModel requestModel)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => 
                x.SaleInvoiceDateTime.Day >= requestModel.FromDate.Day &&
                x.SaleInvoiceDateTime.Month >= requestModel.FromDate.Month &&
                x.SaleInvoiceDateTime.Year == requestModel.FromDate.Year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);

        int TotalCount = query.Count();
        int PageCount = TotalCount / requestModel.PageSetting.PageSize;
        if (TotalCount % requestModel.PageSetting.PageSize != 0)
        {
            PageCount = PageCount + 1;
        }
        var report = await query
                .Pagination(
                requestModel.PageSetting.PageNo,
                requestModel.PageSetting.PageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(
                requestModel.PageSetting.PageNo,
                requestModel.PageSetting.PageSize,
                PageCount, TotalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }
    public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year, int PageNo, int PageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Month == month && x.SaleInvoiceDateTime.Year == year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int TotalCount = query.Count();
        int PageCount = TotalCount / PageSize;
        if (TotalCount % PageSize != 0)
        {
            PageCount = PageCount + 1;
        }

        var report = await query
                .Pagination(PageNo, PageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(PageNo, PageSize, PageCount, TotalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(int year, int PageNo, int PageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();

        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Year == year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int TotalCount = query.Count();
        int PageCount = TotalCount / PageSize;
        if (TotalCount % PageSize != 0)
        {
            PageCount = PageCount + 1;
        }
        var report = await query
                .Pagination(PageNo, PageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(PageNo, PageSize, PageCount, TotalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }
}