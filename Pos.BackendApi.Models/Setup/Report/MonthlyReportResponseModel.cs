using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.BackendApi.Models.Setup.Report;

public class MonthlyReportResponseModel
{
    public List<ReportModel> Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}