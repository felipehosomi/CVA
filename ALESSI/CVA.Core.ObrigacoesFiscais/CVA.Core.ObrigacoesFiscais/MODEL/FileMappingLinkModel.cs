using CVA.AddOn.Common.Controllers;

namespace CVA.Core.ObrigacoesFiscais.MODEL
{
    public class FileMappingLinkModel
    {
        [ModelController(ColumnName = "U_Parent")]
        public string ParentProperty { get; set; }

        [ModelController(ColumnName = "U_Child")]
        public string ChildProperty { get; set; }
    }
}
