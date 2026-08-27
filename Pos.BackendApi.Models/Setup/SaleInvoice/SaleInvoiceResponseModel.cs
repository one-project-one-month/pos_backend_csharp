using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.BackendApi.Models.Setup.SaleInvoice;

public class SaleInvoiceResponseModel
{
    public SaleInvoiceModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}