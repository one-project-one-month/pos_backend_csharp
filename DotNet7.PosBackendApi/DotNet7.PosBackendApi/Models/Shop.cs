using System;
using System.Collections.Generic;

namespace DotNet7.PosBackendApi.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public string ShopCode { get; set; } = null!;

    public string ShopName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string Address { get; set; } = null!;
}
