using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.MODEL
{
    public class ImportLogModel
    {
        [ModelController(ColumnName = "U_Date")]
        public DateTime Date { get; set; }

        [ModelController(ColumnName = "U_Hour")]
        public DateTime Hour { get; set; }

        [ModelController(ColumnName = "U_User")]
        public string User { get; set; }

        [ModelController(ColumnName = "U_Line")]
        public int Line { get; set; }

        [ModelController(ColumnName = "U_Description")]
        public string Description { get; set; }
    }
}
