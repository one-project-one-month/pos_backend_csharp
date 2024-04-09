using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet7.PosBackendApi.Models.Setup.Shop
{
    public class ShotListResponseModel
    {
        public List<ShopModel> DataLst { get; set; }
        public MessageResponseModel MessageResponse { get; set; }
    }
}
