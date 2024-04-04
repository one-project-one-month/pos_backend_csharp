using System;
using System.Collections.Generic;

namespace DotNet7.PosBackendApi.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public string ProductCategoryCode { get; set; } = null!;

    public string ProductCategoryName { get; set; } = null!;
}
