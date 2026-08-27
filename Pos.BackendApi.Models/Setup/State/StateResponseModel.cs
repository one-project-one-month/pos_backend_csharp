using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.BackendApi.Models.Setup.State;

public class StateResponseModel
{
    public StateModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}