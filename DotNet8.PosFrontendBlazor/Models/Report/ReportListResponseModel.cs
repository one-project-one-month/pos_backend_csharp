namespace DotNet8.PosFrontendBlazor.Models.Report;

public class ReportListResponseModel : ResponseModel
{
    public ReportDataModel Data {  get; set; }
    public PageSettingModel PageSetting { get; set; }
}