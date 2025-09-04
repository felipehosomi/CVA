using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.View
{
    public class CriterioComissaoForm
    {
        public const string Type = "CVACRIT";
        public const string ObjType = "UDOCRIT";

        public const string EditCode = "Code";
        public const string EditName = "Name";
        public const string EditPosition = "Pos";
        public const string CheckAtivo = "Ativo";

        public const string ButtonOk = "1";
        public const string ButtonCancelar = "2";

        public const string DbDataSource = "@CVA_CRIT_COMISSAO";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_criterios_comissao.srf";
    }
}
