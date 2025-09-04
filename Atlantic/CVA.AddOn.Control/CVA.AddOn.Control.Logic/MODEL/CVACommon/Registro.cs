using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.MODEL.CVACommon
{
    public class Registro
    {
        public int ID { get; set; }

        [ModelController(ColumnName = "INS")]
        public System.DateTime Insert { get; set; }

        [ModelController(ColumnName = "UPD")]
        public Nullable<System.DateTime> Update { get; set; }

        [ModelController(ColumnName = "STU")]
        public Nullable<int> Status { get; set; }

        [ModelController(ColumnName = "BAS")]
        public Nullable<int> CodigoBase { get; set; }

        [ModelController(ColumnName = "CODE")]
        public string Codigo { get; set; }

        [ModelController(ColumnName = "OBJ")]
        public Nullable<int> Objeto { get; set; }

        [ModelController(ColumnName = "FUNC")]
        public Nullable<int> Funcao { get; set; }

        [ModelController(ColumnName = "BAS_ERR")]
        public Nullable<int> BaseErro { get; set; }
    }
}
