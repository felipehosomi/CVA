using CVA.AddOn.Common.Controllers;
using System.Collections.Generic;

namespace CVA.Core.ObrigacoesFiscais.MODEL
{
    public class FileMappingModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController(ColumnName = "U_Layout")]
        public string Layout { get; set; }

        [ModelController(ColumnName = "U_Child")]
        public string Child { get; set; }

        [ModelController(ColumnName = "U_Description")]
        public string Description { get; set; }

        [ModelController(ColumnName = "U_SqlType")]
        public string ObjectType { get; set; }

        [ModelController(ColumnName = "U_ObjName")]
        public string ObjectName { get; set; }

        [ModelController(ColumnName = "U_Identifier")]
        public string Identifier { get; set; }

        public List<FileMappingItemModel> FieldsList { get; set; }
        public List<FileMappingLinkModel> ParentLinkList { get; set; }
        public List<FileMappingLinkModel> ChildLinkList { get; set; }
    }
}

