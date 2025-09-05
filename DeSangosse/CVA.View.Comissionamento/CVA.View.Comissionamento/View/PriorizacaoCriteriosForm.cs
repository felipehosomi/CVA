using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.View
{
    public class PriorizacaoCriteriosForm
    {
        public const string Type = "CVAPRIC";
        public const string ObjType = "";

        public const string ButtokOk = "1";
        public const string ButtonCancelar = "2";
        public const string ButtonUp = "Item_3";
        public const string ButtonDown = "Item_4";

        public const string MatrixItens = "Item_2";

        public const string ColumnEditLinha = "#";
        public const string ColumnEditCriterioComissao = "Col_0";
        public const string ColumnCheckAtivo = "Col_1";
        public const string ColumnCode = "Col_2";
        public const string ColumnPos = "Col_3";

        public const string UserDataSourceUD_0 = "UD_0";
        public const string UserDataSourceUD_1 = "UD_1";
        public const string UserDataSourceUD_2 = "UD_2";
        public const string UserDataSourceUD_3 = "UD_3";
        public const string UserDataSourceUD_4 = "UD_4";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_priorizacao_criterios.srf";
    }
}
