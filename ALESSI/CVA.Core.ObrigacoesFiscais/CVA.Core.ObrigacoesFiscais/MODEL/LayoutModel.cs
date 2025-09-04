using CVA.AddOn.Common.Controllers;

namespace CVA.Core.ObrigacoesFiscais.MODEL
{
    public class LayoutModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController(ColumnName =  "U_Name")]
        public string Name { get; set; }

        [ModelController(ColumnName = "U_Separator")]
        public string Separator { get; set; }

        [ModelController(ColumnName = "U_Start_Sep")]
        public string StartsWithSeparator { get; set; }

        [ModelController(ColumnName = "U_Char_String")]
        public string PaddingCharString { get; set; }

        [ModelController(ColumnName = "U_Pad_String")]
        public int PaddingDirectionString { get; set; }

        [ModelController(ColumnName = "U_Char_Num")]
        public string PaddingCharNumeric { get; set; }

        [ModelController(ColumnName = "U_Pad_Num")]
        public int PaddingDirectionNumeric { get; set; }

        [ModelController(ColumnName = "U_Decimal_Qty")]
        public int DecimalQuantity { get; set; }

        [ModelController(ColumnName = "U_Decimal_Sep")]
        public string DecimalSeparator { get; set; }

        [ModelController(ColumnName = "U_Date_Format")]
        public string DateFormat { get; set; }

        [ModelController(ColumnName = "U_File_Format")]
        public int FileFormat { get; set; }

        [ModelController(ColumnName = "U_Dir")]
        public string Directory { get; set; }
    }
}
