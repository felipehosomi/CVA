using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.Views
{
    public class MetaComissaoForm
    {
        public const string Type = "CVAMETA";
        public const string ObjType = "UDOMETA";

        public const string EditCode = "et_Code";
        public const string EditDesc = "et_Desc";
        public const string ComboFilial = "cb_Filial";
        public const string ComboTipo = "cb_Tipo";
        public const string EditValor = "et_Valor";
        public const string Matrix = "mt_Meta";
        public const string ColumnDe = "cl_De";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Forms\\cva_meta_comissao.srf";
    }
}
