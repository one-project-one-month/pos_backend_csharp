using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet8.PosBackendApi.Models.Setup.State
{
    public class StateListResponseModel
    {
        //public List<TownshipModel> DataLst { get; set; }
        //public MessageResponseModel MessageResponse { get; set; }

        public List<StateModel> DataLst { get; set; }
        public MessageResponseModel MessageResponse { get; set; }
    }
}
