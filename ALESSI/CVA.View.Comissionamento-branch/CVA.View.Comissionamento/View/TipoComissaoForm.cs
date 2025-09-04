using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.View
{
    public class TipoComissaoForm
    {
        public const string Type = "CVATIPO";
        public const string ObjType = "UDOTIPO";

        public const string EditCode = "Code";
        public const string EditName = "Name";
        public const string CheckTipo = "Tipo";

        public const string ButtonOk = "1";
        public const string ButtonCancelar = "2";

        public const string DbDataSource = "@CVA_TIPO_COMISSAO";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_tipo_comissao.srf";
    }
}
