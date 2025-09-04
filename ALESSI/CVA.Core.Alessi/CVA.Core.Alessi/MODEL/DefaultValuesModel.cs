using CVA.AddOn.Common.Controllers;

namespace CVA.Core.Alessi.MODEL
{
    public class DefaultValuesModel
    {
        [ModelController(ColumnName = "U_DI_Obj")]
        public int DiObj { get; set; }

        [ModelController(ColumnName = "U_DI_Type")]
        public int DiType { get; set; }

        [ModelController(ColumnName = "U_DI_Field")]
        public string DiField { get; set; }

        [ModelController(ColumnName = "U_Value")]
        public string Value { get; set; }
    }
}
