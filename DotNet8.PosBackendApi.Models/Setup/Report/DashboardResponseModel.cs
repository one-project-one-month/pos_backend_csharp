using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.Report
{
    public class DashboardResponseModel
    {
        public List<DailyDashboardModel> DailyData { get; set; }
        public List<WeeklyDashboardModel> WeeklyData { get; set; }
        public List<MonthlyDashboardModel> MonthlyData { get; set; }
        public List<YearlyDashboardModel> YearlyData { get; set; }
        public List<BestSellerProductDashboardModel> BestProductData { get; set; }
        public MessageResponseModel MessageResponse { get; set; }
        public PageSettingModel PageSetting { get; set; }
    }

    public class DailyDashboardModel
    {
        public DateTime SaleInvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
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
}
