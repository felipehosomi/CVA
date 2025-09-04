using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.MODEL.CVACommon
{
    public class Objeto
    {
        [ModelController(ColumnName = "ID")]
        public int ID { get; set; }

        [ModelController(ColumnName = "INS")]
        public System.DateTime Insert { get; set; }

        [ModelController(ColumnName = "UPD")]
        public Nullable<System.DateTime> Update { get; set; }

        [ModelController(ColumnName = "OBJ")]
        public string NomeObjeto { get; set; }

        [ModelController(ColumnName = "DSCR")]
        public string Descricao { get; set; }

        [ModelController(ColumnName = "STU")]
        public Nullable<int> Status { get; set; }
    }
}
