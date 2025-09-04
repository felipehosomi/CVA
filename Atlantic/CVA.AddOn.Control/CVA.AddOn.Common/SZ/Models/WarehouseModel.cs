using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZS.Common.Controllers;

namespace SZS.Common.Models
{
    public class WarehouseModel
    {
        [ModelController]
        public string WhsCode { get; set; }

        [ModelController]
        public string WhsName { get; set; }
    }
}
