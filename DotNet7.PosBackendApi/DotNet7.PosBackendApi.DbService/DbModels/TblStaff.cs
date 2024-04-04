using System;
using System.Collections.Generic;

namespace DotNet7.PosBackendApi.DbService.DbModels;

public partial class TblStaff
{
    public int StaffId { get; set; }

    public string StaffCode { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string MobileNo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Position { get; set; } = null!;
}
