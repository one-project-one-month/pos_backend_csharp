using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.State;

public class StateModel
{
    public int StateId { get; set; }

    public string? StateCode { get; set; } 

    public string StateName { get; set; }
}