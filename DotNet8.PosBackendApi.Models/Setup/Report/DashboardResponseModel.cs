using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.Report;

public class DashboardResponseModel
{
    public List<DailyDashboardModel> DailyData { get; set; } = new List<DailyDashboardModel>();
    public List<WeeklyDashboardModel> WeeklyData { get; set; } = new List<WeeklyDashboardModel>();
    public List<MonthlyDashboardModel> MonthlyData { get; set; } = new List<MonthlyDashboardModel>();
    public List<YearlyDashboardModel> YearlyData { get; set; } = new List<YearlyDashboardModel>();
    public List<BestSellerProductDashboardModel> BestProductData { get; set; } = new List<BestSellerProductDashboardModel>();
    public MessageResponseModel MessageResponse { get; set; } = new MessageResponseModel();
    public PageSettingModel PageSetting { get; set; }
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

public class BestSellerProductDashboardModel
{
    public string ProductName { get; set; }
    public int TotalQty { get; set; }
}