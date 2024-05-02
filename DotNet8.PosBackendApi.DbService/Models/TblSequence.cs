using System;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.DbService.Models;

public partial class TblSequence
{
    public int Id { get; set; }

    public string Field { get; set; } = null!;

    public string Code { get; set; } = null!;

    public int Length { get; set; }

    public int Sequence { get; set; }
}
