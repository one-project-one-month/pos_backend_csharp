namespace DotNet8.PosFrontendBlazor.Server.Models.Report;

public class ReportListResponseModel : ResponseModel
{
    public ReportDataModel Data {  get; set; }
    public PageSettingModel PageSetting { get; set; }
}