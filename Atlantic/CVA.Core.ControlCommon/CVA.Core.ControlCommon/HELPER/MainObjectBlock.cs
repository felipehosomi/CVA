using CVA.Core.ControlCommon.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.HELPER
{
    public class MainObjectBlock
    {
        public static List<int> ObjectList
        {
            get
            {
                return new List<int>
                {
                    (int)CVAObjectEnum.CodigoImposto,
                    (int)CVAObjectEnum.Indicador,
                    (int)CVAObjectEnum.PlanoContas,
                };
            }
        }
    }
}
