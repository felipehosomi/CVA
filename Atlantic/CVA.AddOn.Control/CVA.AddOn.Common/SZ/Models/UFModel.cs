using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZS.Common.Controllers;

namespace SZS.Common.Models
{
    public class UFModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController]
        public string Name { get; set; }
    }
}
