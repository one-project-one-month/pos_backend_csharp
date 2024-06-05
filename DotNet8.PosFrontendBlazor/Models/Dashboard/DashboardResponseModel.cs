namespace DotNet8.PosFrontendBlazor.Models.Dashboard;

public class DashboardResponseModel : ResponseModel
{
    public string Message { get; set; }
    public string Token { get; set; }
    public bool IsSuccess { get; set; }
    public Data Data { get; set; }
}

public class Data
{
    public Dashboard Dashboard { get; set; }
}

public class Dashboard
{
    public List<DailyDashboardModel> DailyData { get; set; }
    public List<WeeklyDashboardModel> WeeklyData { get; set; }
    public List<MonthlyDashboardModel> MonthlyData { get; set; }
    public List<YearlyDashboardModel> YearlyData { get; set; }
    public List<BestSellerProduct> BestSellerProduct { get; set; }
}

public class DailyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class WeeklyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class MonthlyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class YearlyDashboardModel
{
    public int Year { get; set; }
    public decimal Amount { get; set; }
}

public class BestSellerProduct
{
    public string ProductName { get; set; }
    public int TotalQty { get; set; }
}
