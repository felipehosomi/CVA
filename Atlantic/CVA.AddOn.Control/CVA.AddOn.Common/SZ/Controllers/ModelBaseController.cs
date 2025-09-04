using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZS.Common.Enums;

namespace SZS.Common.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelBaseControllerAttribute : Attribute
    {
        public EnumModelTableType EnumModelTableType { get; set; }
    }
}
