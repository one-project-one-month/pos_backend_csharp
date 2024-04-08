using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet7.PosBackendApi.Models.Setup.Shop
{
    public class ShopModel
    {
        public int ShopId { get; set; }

        public string ShopCode { get; set; } = null!;

        public string ShopName { get; set; } = null!;

        public string MobileNo { get; set; } = null!;   

        public string Address { get; set; } = null!;
    }
}
