using CVA.AddOn.Common.Controllers;

namespace CVA.Core.Alessi.MODEL
{
    public class DocumentMappingItemModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController]
        public int LineId { get; set; }

        [ModelController(ColumnName = "U_DI_Field")]
        public string DiField { get; set; }

        [ModelController(ColumnName = "U_DI_Type")]
        public int DiType { get; set; }

        [ModelController(ColumnName = "U_From")]
        public int PositionFrom { get; set; }

        [ModelController(ColumnName = "U_To")]
        public int PositionTo { get; set; }

        [ModelController(ColumnName = "U_Size")]
        public int Size { get; set; }

        [ModelController(ColumnName = "U_Decimal")]
        public int DecimalPlaces { get; set; }

        [ModelController(ColumnName = "U_Format")]
        public string Format { get; set; }

        [ModelController(ColumnName = "U_Parameter")]
        public string Parameter { get; set; }

        [ModelController(ColumnName = "U_Query")]
        public string Query { get; set; }

        public object ParameterValue { get; set; }
    }
}
