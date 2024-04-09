using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet7.PosBackendApi.Models.Setup.Shop
{
    public class ShopResponseModel
    {
        public ShopModel Data { get; set; }
        public MessageResponseModel MessageResponse { get; set; }
    }
}
