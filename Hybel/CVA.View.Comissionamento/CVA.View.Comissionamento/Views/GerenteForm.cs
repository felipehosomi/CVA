using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.Views
{
    public class GerenteForm
    {
        public const string Type = "CVAGRNT";
        public const string ObjType = "UDOGRNT";

        public const string EditCode = "et_Code";
        public const string EditNome = "et_Nome";
        public const string Matrix = "mt_Item";
        public const string ColumnId = "cl_Id";
        public const string ColumnNome = "cl_Nome";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Forms\\cva_gerente.srf";
    }
}
