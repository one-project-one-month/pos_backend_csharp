using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.BackendApi.Models.Setup.State;

public class StateModel
{
    public int StateId { get; set; }

    public string? StateCode { get; set; } 

    public string StateName { get; set; }
}