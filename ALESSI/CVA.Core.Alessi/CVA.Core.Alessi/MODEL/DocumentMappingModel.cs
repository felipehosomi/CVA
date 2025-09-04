using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.MODEL
{
    public class DocumentMappingModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController(ColumnName = "U_ObjType")]
        public int ObjType { get; set; }

        [ModelController(ColumnName = "U_LineType")]
        public int LineType { get; set; }

        [ModelController(ColumnName = "U_Identifier")]
        public string LineIdentifier { get; set; }

        public List<DocumentMappingItemModel> FieldsList { get; set; }
    }
}
