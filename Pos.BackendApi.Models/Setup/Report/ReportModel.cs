using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.Report;

public class ReportModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
}