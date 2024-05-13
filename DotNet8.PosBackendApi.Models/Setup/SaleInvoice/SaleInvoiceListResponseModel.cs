using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.SaleInvoice
{
    public class SaleInvoiceListResponseModel
    {
        public List<SaleInvoiceModel> DataList { get; set; } = new List<SaleInvoiceModel>();
        public MessageResponseModel MessageResponse { get; set; }
    }
}
