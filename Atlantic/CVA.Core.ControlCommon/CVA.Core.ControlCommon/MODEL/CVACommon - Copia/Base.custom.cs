using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.MODEL.CVACommon
{
    public partial class Base
    {
        public bool IsMain
        {
            get
            {
                return ID == 1;
            }
        }
    }
}
