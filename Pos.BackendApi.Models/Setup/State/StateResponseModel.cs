using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.State;

public class StateResponseModel
{
    public StateModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
}