namespace DotNet8.PosBackendApi.Models.Setup.Report;

public class SaleDailyReportRequestModel
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }   
    public PageSettingModel PageSetting { get; set; }
}
