using System;
using System.Collections.Generic;

namespace DotNet7.PosBackendApi.DbService.Models;

public partial class TblProductCategory
{
    public int ProductCategoryId { get; set; }

    public string ProductCategoryCode { get; set; } = null!;

    public string ProductCategoryName { get; set; } = null!;
}
