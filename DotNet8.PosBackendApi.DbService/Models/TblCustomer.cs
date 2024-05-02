using System;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.DbService.Models;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string TownshipCode { get; set; } = null!;
}
