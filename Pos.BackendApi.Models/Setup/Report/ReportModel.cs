using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.BackendApi.Models.Setup.Report;

public class ReportModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
}