using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.SaleInvoice
{
    public class SaleInvoiceModel
    {
        public string VoucherNo { get; set; }
        public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; }
    }
}
